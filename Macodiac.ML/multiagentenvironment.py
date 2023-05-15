import gymnasium as gym
from gym import Env
# use `gym.spaces` here, even though we're using `gymnasium`
# https://stackoverflow.com/questions/75108957/assertionerror-the-algorithm-only-supports-class-gym-spaces-box-box-as-acti
from gym import spaces
import numpy as np
import random
from stable_baselines3.common.callbacks import BaseCallback

class AgentObject:
    def __init__(self):
        self.state = []
        self.vendingPrice = 0
        self.reward = 0
        self.quantitySold = 0

class ConsumerObject:
    def __init__(self):
        self.demand = 1
        self.utility = 0
        self.money = 0
        self.total_consumed = 0


class TensorboardPriceCallback(BaseCallback):
    """ 
    custom logger to record the price charged by agents
    """
    # iterator = 0
    # offeredPxList = []
    # acceptedPxList = []
    # vendorsMadeSaleList = []
    # quantitySoldList = []


    runningAvgMeanPxOffered = 0
    runningAvgAcceptedVendedPx = 0

    def __init__(self, verbose=0):
        self.reset()
        super().__init__(verbose)

    def reset(self):
        self.iterator = 0

    def _on_rollout_end(self) -> None:
        self.reset()
        return super()._on_rollout_end()
    
    def _on_step(self) -> bool:
        # self.iterator +=1
        # agent_arr = self.training_env.get_attr('policy_agents')[0]
        
        ### generate a dict like:
        # [{'agent_num': 0, 'price': 10, 'sold': 0.0, 'reward': 0.0}, 
        # {'agent_num': 1, 'price': 10, 'sold': 50.0, 'reward': 0.0}]
        info_arr = self.locals['infos'][0]['n']


        pxList = []
        vendorsMadeSale = 0
        quantitySold = 0
        countNoSale = 0
        countWiSale = 0
        acceptedVendedPx = 0
        meanPxOffered = 0

        for agentInfo in info_arr:
            agent_sales = agentInfo['sold']
            agent_vend_px = agentInfo['price']
            pxList.append(agent_vend_px)
            
            if agent_sales > 0:
                vendorsMadeSale += 1
                quantitySold += agent_sales
                countWiSale += 1
                acceptedVendedPx = agent_vend_px
            else:
                countNoSale += 1
            
            self.logger.record(f'vending_agent_{agentInfo["agent_num"]}/offered_px',   agent_vend_px)
            self.logger.record(f'vending_agent_{agentInfo["agent_num"]}/sales_complete',  agent_sales)
            self.logger.record(f'vending_agent_{agentInfo["agent_num"]}/individual_reward',  agentInfo['reward'])
            
        meanPxOffered = np.mean(pxList)

        self.logger.record('vending/avgerage_offered_px_value', meanPxOffered)
        self.logger.record('vending/actual_accepted_px_value', acceptedVendedPx)
        self.logger.record('vending/quantity_sold_count', quantitySold)
        self.logger.record('vending/vendors_made_sale_count', vendorsMadeSale)
        self.logger.record('vending/count_no_sale', countNoSale)
        self.logger.record('vending/count_wi_sale', countWiSale)

        return True

class MultiAgentMacodiacEnvironment(Env):
    """
    Builds a profit maximising agent environment, supporting
    up to n_agent agents
    """
    state = 0
    environment_timesteps = 0
    environment_starter_timesteps = 150
    env_wholesale_price = 5        # the price agents pay to purchase goods
    env_agent_marginal_cost = 0     # the marginal cost of vending
    num_consumers = 25
    consumer_total_money_per_turn = 250
    consumers_arr = []


    def __init__(self, envTimesteps:int, numAgents: int):
        """
        Initialises the class
        """
        self.environment_starter_timesteps = envTimesteps
        self.policy_agents = []
        self.observation_space = []

        for i in range(numAgents):
            self.policy_agents.append(AgentObject())
        
        for i in range(self.num_consumers):
            self.consumers_arr.append(ConsumerObject())

        
        # creates an array full of 10's shaped [20,20,20], of length numAgents
        self.action_space = spaces.MultiDiscrete(np.full(numAgents, 10) )


        # the observation space is a nAgents by nActions array of float32 numbers between -99-99
        # also contains the wholesale price
        # Observations space:
        # 0: agent's state, after the action has been applied
        # 1: agent's vending price in this round
        # 2: agent's count of sold items
        # 3: the wholesale price in this round
        self.observation_space = spaces.Box(low=0,high=200, shape=(numAgents, 3), dtype=np.int32)

        print(f'obs_space.sample: {self.observation_space.sample()}')

        self.reset()


        
        print('-- ENV SETTINGS --')
        print(f'obs:{self.observation_space}')
        print(f'sample:{self.observation_space.sample()}')
        print(self.action_space)
        print(self.action_space.sample())
        print(self.environment_timesteps)
        print('-- ENV SETTINGS --')


    def set_agent_action(self, action, agent, actionSpace):
        # agent.state is the percentage price diff from the wholesale price
        agent.state = action
        agent.vendingPrice = self.env_wholesale_price + agent.state

        if agent.vendingPrice == 0:
            print(f'error')
            agent.vendingPrice = max(1, agent.vendingPrice)


        # agentBaseVendingPriceAdjust = self.env_wholesale_price * (agent.state / 100)
        # baseAgentVendingPrice = self.env_wholesale_price + agentBaseVendingPriceAdjust
        # #agentMarginalCostAddedVendingPrice = agentBaseVendingPriceAdjust + self.env_agent_marginal_cost
        # agent.vendingPrice = max(baseAgentVendingPrice, 1)
        # print(f'agent vending price was {agent.vendingPrice}')

    def step_agent(self, agent):
        reward = agent.reward
        info = {}
        return agent.state, reward, False, info


    def step(self, action_arr):
        """
        Processes an action for an agent.

        Loops through each agent and sets its action. 
        Then calls world.step to progress the entire world's actions

        Builds up arrays of results, and returns them in a tuple of arrays

        """
        self.environment_timesteps -=1

        obs_arr = []
        reward_arr = []
        done_arr = []
        info_arr = {'n': []}
        
        for i, agent in enumerate(self.policy_agents):
            self.set_agent_action(action_arr[i], agent, self.action_space[i])

        for i, consumer in enumerate(self.consumers_arr):
            self.set_consumer_purchases(self.policy_agents, consumer)
        
        anyConsumed = False
        for consumer in self.consumers_arr:
            if consumer.total_consumed > 0:
                anyConsumed = True
                break
        if anyConsumed == False:
            print(f'error, no consumption')

        for i, agent in enumerate(self.policy_agents):
            agent.state, agent.reward, agent.done, agent.info = self.step_agent(agent)
            obs_arr.append(self._get_obs(agent))
            reward_arr.append(self._get_reward(agent))
            done_arr.append(self._get_done(agent))
            info_arr['n'].append(self._get_info(agent, i))

        # for agent in self.policy_agents:
           

        if self.environment_timesteps <= 0:
            isTerminal = True
        elif any(done_arr):
             isTerminal = True
        else:
            isTerminal = False

        tmpObsArray = []
        for i, agent in enumerate(self.policy_agents):
            partialObservationResult = self.get_agent_default_observation_array()
            partialObservationResult[0] = self._get_obs(agent) #The agent's result is present in the 0th element of its result
            partialObservationResult[1] = self._get_final_vend_price(agent) #The agent's result is present in the 0th element of its result
            partialObservationResult[2] = self._get_quantity_sold(agent) #The agent's result is present in the 0th element of its result
            tmpObsArray.append(partialObservationResult)

        concatObsArray = np.array(tmpObsArray).astype(np.int32) 
        return concatObsArray, sum(reward_arr), isTerminal,  info_arr


    def set_consumer_purchases(self, agents_arr, consumer):
        """
        So long as the consumer has money, loops through the agents, and selects the lowest
        price agent. 

        Purchases as many items from the agent as possible
        """
        lowestPriceAgnetIndex = 0
            
        for i, agent in enumerate(agents_arr):
            if agent.vendingPrice < agents_arr[lowestPriceAgnetIndex].vendingPrice:
                lowestPriceAgnetIndex = i

        lowestAgentVendPrice = agents_arr[lowestPriceAgnetIndex].vendingPrice

        # instead of this while loop, just return the 
        quantityPurchasable = np.floor(consumer.money / lowestAgentVendPrice)
        consumerConsumed = quantityPurchasable
        tmpAgentRewardPerUnitSold = (lowestAgentVendPrice - self.env_wholesale_price)
        agentReward = tmpAgentRewardPerUnitSold * consumerConsumed

        consumer.money = 0
        consumer.total_consumed += consumerConsumed
        agents_arr[lowestPriceAgnetIndex].reward += agentReward
        agents_arr[lowestPriceAgnetIndex].quantitySold += consumerConsumed



    def _get_quantity_sold(self, agent):
        """
            accepts an agent, and returns the number of items it sold
        """
        return agent.quantitySold

    def _get_final_vend_price(self, agent):
        """
            accepts an agent and returns its final vending
        """
        return agent.vendingPrice
    
    def _get_obs(self, agent):
        """
            accepts an Agent, and returns its observation/state
        """
        return agent.state

    def _get_reward(self, agent):
        """
            accepts an Agent, and returns its reward
        """
        return agent.reward

    def _get_done(self, agent):
        """
            accepts an Agent, and returns its done/terminal property
        """
        return agent.done

    def _get_info(self, agent , i):
        """
            accepts an Agent, and returns its info object
        """
        return {"agent_num": i, "price": agent.vendingPrice, "sold": agent.quantitySold, "reward": agent.reward}


    def render(self) -> None:
        """
        Does nothing, the environment is fully headless
        """
        pass

    def reset(self): #-> float:
        """
        Sets the application to its initial conditions

        Sets state to a random float between negative 100 to  positive 100
        """
        obs_arr =[]
        for i in range(len(self.policy_agents)):
            self.policy_agents[i].state = np.array(
                                    self.get_agent_default_observation_array(), 
                                     dtype=np.int32)
            obs_arr.append(self.policy_agents[i].state)
            self.policy_agents[i].reward = 0
            self.policy_agents[i].info = {}
            self.policy_agents[i].done = False
            self.policy_agents[i].vendingPrice = 0
            self.policy_agents[i].quantitySold = 0
        
        consumerMoneyEach = self.consumer_total_money_per_turn / self.num_consumers
        for i in range(len(self.consumers_arr)):
            self.consumers_arr[i].money = consumerMoneyEach

        self.environment_timesteps = self.environment_starter_timesteps
        
        return np.array(obs_arr).astype(np.int32)


    def get_agent_default_observation_array(self):
        """
        Gets a default observation for this space
        """
        return [0.0, 0.0, 0]# , self.env_wholesale_price]