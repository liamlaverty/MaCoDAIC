import gymnasium as gym
from gym import Env
# use `gym.spaces` here, even though we're using `gymnasium`
# https://stackoverflow.com/questions/75108957/assertionerror-the-algorithm-only-supports-class-gym-spaces-box-box-as-acti
from gym import spaces
import numpy as np
import random
from random import shuffle
from stable_baselines3.common.callbacks import BaseCallback

class AgentObject:
    def __init__(self):
        self.state = []
        self.reset_values()
    
    def reset_values(self):
        self.vendingPrice = 0
        self.reward = 0
        self.quantitySold = 0
        self.willingToSellMaximum = 0
        self.vendCost = 5
        self.totalVendingCost = 0
        self.vendCostTrend = 'down'

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
        self.reset()
        # self.iterator +=1
        # agent_arr = self.training_env.get_attr('policy_agents')[0]
        
        ### generate a dict like:
        # [{'agent_num': 0, 'price': 10, 'sold': 0.0, 'reward': 0.0}, 
        # {'agent_num': 1, 'price': 10, 'sold': 50.0, 'reward': 0.0}]
        info_arr = self.locals['infos'][0]['n']


        pxList = []
        acceptedPxList = []
        vendCostList = []
        quantityOfferedList = []
        vendorsMadeSale = 0
        quantitySold = 0
        countNoSale = 0
        countWiSale = 0
        meanPxOffered = 0
        agent_sales = 0
        agent_vend_px = 0
        agent_reward = 0
        money_sales = 0
        agent_sales_in_money = 0
        agent_total_vend_cost = 0
        agent_final_vend_cost = 0
        agent_quantity_offered = 0
        meanPxAccepted = 0
        meanVendCost = 0
        meanquantityOffered = 0
         
        for i, agentInfo in enumerate(info_arr):
            agent_sales = info_arr[i]['sold']
            agent_vend_px = info_arr[i]['price']
            agent_reward = info_arr[i]['reward']
            agent_final_vend_cost = info_arr[i]['vendCost']
            agent_total_vend_cost = info_arr[i]['totalVendingCost']
            agent_quantity_offered = info_arr[i]['totalQuantityOffered']
            agent_sales_in_money = agent_sales * agent_vend_px
            money_sales += agent_sales_in_money

            pxList.append(agent_vend_px)
            
            if agent_sales > 0:
                vendorsMadeSale += 1
                quantitySold += agent_sales
                countWiSale += 1
                acceptedPxList.append(agent_vend_px)
                vendCostList.append(agent_final_vend_cost)
                quantityOfferedList.append(agent_quantity_offered)
            else:
                countNoSale += 1
            
            self.logger.record(f'a_vending_agent_{agentInfo["agent_num"]}/offered_px',   agent_vend_px)
            self.logger.record(f'a_vending_agent_{agentInfo["agent_num"]}/sales_complete',  agent_sales)
            self.logger.record(f'a_vending_agent_{agentInfo["agent_num"]}/sales_value',  agent_sales_in_money)
            self.logger.record(f'a_vending_agent_{agentInfo["agent_num"]}/final_vend_cost',  agent_final_vend_cost)
            self.logger.record(f'a_vending_agent_{agentInfo["agent_num"]}/total_vend_cost',  agent_total_vend_cost)
            self.logger.record(f'a_vending_agent_{agentInfo["agent_num"]}/quantity_offered',  agent_quantity_offered)
            self.logger.record(f'a_vending_agent_{agentInfo["agent_num"]}/individual_reward',  agent_reward)
            
        if len(pxList) > 0:
            meanPxOffered = np.mean(pxList)
        if len(acceptedPxList) > 0:
            meanPxAccepted = np.mean(acceptedPxList)
        if len(vendCostList) > 0:
            meanVendCost = np.mean(vendCostList)
        if len(quantityOfferedList) > 0:
            meanquantityOffered = np.mean(quantityOfferedList)


        self.logger.record('a_vending/avgerage_offered_px_value', meanPxOffered)
        self.logger.record('a_vending/average_accepted_px_value', meanPxAccepted)
        self.logger.record('a_vending/average_final_vend_cost', meanVendCost)
        self.logger.record('a_vending/average_quantity_offered', meanquantityOffered)

        self.logger.record('a_vending/quantity_sold_count', quantitySold)
        self.logger.record('a_vending/total_value_sold', money_sales)
        self.logger.record('a_vending/vendors_made_sale_count', vendorsMadeSale)
        self.logger.record('a_vending/count_no_sale', countNoSale)
        self.logger.record('a_vending/count_wi_sale', countWiSale)

        return True

class MultiAgentMacodiacEnvironment(Env):
    """
    Builds a profit maximising agent environment, supporting
    up to n_agent agents
    """
    state = 0
    environment_timesteps = 0
    environment_starter_timesteps = 150
    env_wholesale_price = 8        # the price agents pay to purchase goods
    env_agent_marginal_cost = 0     # the marginal cost of vending
    num_consumers = 25
    consumer_total_money_per_turn = 475
    consumers_arr = []


    def __init__(self, envTimesteps:int, numAgents: int):
        """
        Initialises the class
        """
        self.environment_starter_timesteps = envTimesteps
        self.policy_agents = []
        self.consumers_arr = []
        self.observation_space = []

        for i in range(numAgents):
            self.policy_agents.append(AgentObject())
        
        for i in range(self.num_consumers):
            self.consumers_arr.append(ConsumerObject())

        
        # creates an array full of 30's shaped [30,30,30], of length numAgents muliplied by 2 (two actions per agent)
        self.action_space = spaces.MultiDiscrete(np.full((numAgents * 2), 30) )


        # the observation space is a nAgents by nActions array of float32 numbers between -99-99
        # also contains the wholesale price
        # Observations space:
        # 0: agent's state, after the action has been applied
        # 1: agent's vending price in this round
        # 2: number of items the agent was willing to sell
        # 2: agent's actual count of sold items
        # 3: the wholesale price in this round
        self.observation_space = spaces.Box(low=0,high=200, shape=(numAgents, 4), dtype=np.int32)

        print(f'obs_space.sample: {self.observation_space.sample()}')

        self.reset()


        
        print('-- ENV SETTINGS --')
        print(f'obs:{self.observation_space}')
        print(f'sample:{self.observation_space.sample()}')
        print(self.action_space)
        print(self.action_space.sample())
        print(self.environment_timesteps)
        print('-- ENV SETTINGS --')

    def peek_env_consumer_money(self):
        return self.consumer_total_money_per_turn
    def peek_env_consumer_money_each(self):
        return self.consumerMoneyEach

    def clear_consumer_stats(self, consumer):
        consumer.money = self.consumerMoneyEach

    def clear_agent_stats(self, agent):
        agent.reset_values()
        


    def set_agent_action(self, actionPrice, actionQuantity, agent):
        # agent.state is the percentage price diff from the wholesale price
        agent.state = actionPrice
        agent.vendingPrice = self.env_wholesale_price + agent.state
        agent.willingToSellMaximum = actionQuantity

        if agent.vendingPrice == 0:
            print(f'error')
            agent.vendingPrice = max(1, agent.vendingPrice)

        # print (f'agent price/quantity = {actionPrice}/{actionQuantity}')


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
        totalNumAgents = len(self.policy_agents)
        
        for i, agent in enumerate(self.policy_agents):
            priceIndex = i
            quantityIndex = i + totalNumAgents
            self.clear_agent_stats(agent)
            self.set_agent_action(action_arr[priceIndex], action_arr[quantityIndex], agent)

        for i, consumer in enumerate(self.consumers_arr):
            self.clear_consumer_stats(consumer)
            self.alt_set_consumer_purchases(self.policy_agents, consumer)

        for i, agent in enumerate(self.policy_agents):
            agent.state, agent.reward, agent.done, agent.info = self.step_agent(agent)
            obs_arr.append(self._get_obs(agent))
            reward_arr.append(self._get_reward(agent))
            done_arr.append(self._get_done(agent))
            info_arr['n'].append(self._get_info(agent, i))        
 
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
        return concatObsArray, float(sum(reward_arr)), isTerminal,  info_arr


    def alt_set_consumer_purchases(self, agents_arr, consumer):
        """
        So long as the consumer has money, loops through the agents, and selects the lowest
        priced agent. Takes into consideration the Agent's willingness-to-supply

        if multiple agents share the same price points, distributes the sales across them all
        """
        while consumer.money > 0:
            lowestAbsolutePrice = 0
            vendingPrices = []
            lowestPriceAgentIndexList = []

            for i, agent in enumerate(agents_arr):
                if agent.willingToSellMaximum > agent.quantitySold:
                    vendingPrices.append(agent.vendingPrice)
            
            if len(vendingPrices) <= 0:
                # if there are no more vendors willing to sell, exit the while loop
                # print(f'no willing vendors left, exiting for consumer')
                return
                # break


            lowestAbsolutePrice = min(vendingPrices)

            for i, agent in enumerate(agents_arr):
                if agent.vendingPrice == lowestAbsolutePrice:
                    lowestPriceAgentIndexList.append(i)
                
            if len(lowestPriceAgentIndexList) > 0:
                shuffle(lowestPriceAgentIndexList)

                for agentIndex in lowestPriceAgentIndexList:
                    if consumer.money > 0:
                        agentToPurchaseFrom = agents_arr[agentIndex]

                        if agentToPurchaseFrom.vendingPrice != lowestAbsolutePrice:
                            raise ValueError(f'agent vending price [{agentToPurchaseFrom.vendingPrice}] is not the same as lowestAbsPrice:[{lowestAbsolutePrice}]')

                        if consumer.money >= agentToPurchaseFrom.vendingPrice:
                            if agentToPurchaseFrom.willingToSellMaximum > agentToPurchaseFrom.quantitySold:
                                consumer.money -= agentToPurchaseFrom.vendingPrice
                                consumer.total_consumed += 1
                                agentToPurchaseFrom.quantitySold += 1

                                # Marginal cost trends down towards 1, then increases upwards
                                if agentToPurchaseFrom.vendCostTrend == 'up':
                                    agentToPurchaseFrom.vendCost += 0.66
                                elif agentToPurchaseFrom.vendCostTrend == 'down':
                                    agentToPurchaseFrom.vendCost -= 0.66
                                    if agentToPurchaseFrom.vendCost < 1:
                                        agentToPurchaseFrom.vendCostTrend = 'up'
                                
                                agentToPurchaseFrom.totalVendingCost += agentToPurchaseFrom.vendCost 
                                agentToPurchaseFrom.reward += (agentToPurchaseFrom.vendingPrice - self.env_wholesale_price - agentToPurchaseFrom.vendCost)
                                if consumer.money <= 0 or consumer.money < lowestAbsolutePrice:
                                    # the consumer can no longer afford the lowest price item
                                    return
                            else:
                                # the agent is no longer willing to sell
                                break
                        else:
                            # print(f'consumer money was {consumer.money}, setting to 0')
                            consumer.money = 0
                            return
        else:
            print(f'consumer money is no longer above 0:{consumer.money}')

    def set_consumer_purchases(self, agents_arr, consumer):
        """
        So long as the consumer has money, loops through the agents, and selects the lowest
        priced agent. 

        if multiple agents share the same price points, distributes the sales across them all
        """
        lowestAbsolutePrice = 0
        lowestPriceAgentIndexList = []
        vendingPrices = []

        for i, agent in enumerate(agents_arr):
            if agent.willingToSellMaximum > agent.quantitySold:
                vendingPrices.append(agent.vendingPrice)

      
        # print(f'prices are: {vendingPrices}')
        lowestAbsolutePrice = min(vendingPrices)
       
        # gather all of the lowest price agents
        for i, agent in enumerate(agents_arr):
            if agent.vendingPrice == lowestAbsolutePrice and agent.willingToSellMaximum > agent.quantitySold:
                lowestPriceAgentIndexList.append(i)

        shuffle(lowestPriceAgentIndexList)

        # while the consumer still has money, purchase
        # items from the vendors
        if len(lowestPriceAgentIndexList) > 0:
            while consumer.money > 0:
                # loop through each vendor, purchase one item from them
                for agentIndex in lowestPriceAgentIndexList:
                    if consumer.money > 0:
                        agentToPurchaseFrom = agents_arr[agentIndex]
                        
                        if agentToPurchaseFrom.vendingPrice != lowestAbsolutePrice:
                            raise ValueError(f'agent vending price [{agentToPurchaseFrom.vendingPrice}] is not the same as lowestAbsPrice:[{lowestAbsolutePrice}]')

                        if consumer.money >= agentToPurchaseFrom.vendingPrice:
                            if consumer.money < agentToPurchaseFrom.vendingPrice:
                                raise ValueError(f'consumer with: [{consumer.money}] money attempted to purchase from agent charging: [{agentToPurchaseFrom.vendingPrice}]')
                            consumer.money -= agentToPurchaseFrom.vendingPrice
                            # print(f'consumer money: {consumer.money}')
                            consumer.total_consumed += 1
                            agentToPurchaseFrom.quantitySold += 1

                            # Marginal cost trends down towards 1, then increases upwards
                            if agentToPurchaseFrom.vendCostTrend == 'up':
                                agentToPurchaseFrom.vendCost += 0.66
                            elif agentToPurchaseFrom.vendCostTrend == 'down':
                                agentToPurchaseFrom.vendCost -= 0.66
                                if agentToPurchaseFrom.vendCost < 1:
                                    agentToPurchaseFrom.vendCostTrend = 'up'
                            
                            agentToPurchaseFrom.totalVendingCost += agentToPurchaseFrom.vendCost 
                            agentToPurchaseFrom.reward += (agentToPurchaseFrom.vendingPrice - self.env_wholesale_price - agentToPurchaseFrom.vendCost)
                        else:
                            # print(f'consumer money was {consumer.money}, setting to 0')
                            consumer.money = 0
                            break


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
        return {
            "agent_num": i,
            "price": agent.vendingPrice,
            "sold": agent.quantitySold,
            "reward": agent.reward,
            "vendCost": agent.vendCost,
            "totalVendingCost": agent.totalVendingCost,
            "totalQuantityOffered": agent.willingToSellMaximum
        }


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
            self.policy_agents[i].willingToSellMaximum = 0
            self.policy_agents[i].vendingPrice = 0
            self.policy_agents[i].quantitySold = 0
            self.policy_agents[i].vendCost = 1

        
        self.consumerMoneyEach = self.consumer_total_money_per_turn / self.num_consumers
        # for i in range(len(self.consumers_arr)):
            # self.clear_consumer_stats(self.consumers_arr[i])
            # self.consumers_arr[i].money = consumerMoneyEach

        self.environment_timesteps = self.environment_starter_timesteps
        
        return np.array(obs_arr).astype(np.int32)


    def get_agent_default_observation_array(self):
        """
        Gets a default observation for this space
        """
        return [0.0, 0.0, 0.0, 0]# , self.env_wholesale_price]