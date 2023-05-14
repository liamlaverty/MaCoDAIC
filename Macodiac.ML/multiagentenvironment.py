import gymnasium as gym
from gym import Env
# use `gym.spaces` here, even though we're using `gymnasium`
# https://stackoverflow.com/questions/75108957/assertionerror-the-algorithm-only-supports-class-gym-spaces-box-box-as-acti
from gym import spaces
import numpy as np
import random

class AgentObject:
    def __init__(self):
        self.state = []
        self.vendingPrice = 0
        self.reward = 0

class ConsumerObject:
    def __init__(self):
        self.demand = 1
        self.utility = 0
        self.money = 0
        self.total_consumed = 0

class MultiAgentMacodiacEnvironment(Env):
    """
    Builds a profit maximising agent environment, supporting
    up to n_agent agents
    """
    state = 0
    environment_timesteps = 0
    environment_starter_timesteps = 150
    env_wholesale_price = 50        # the price agents pay to purchase goods
    env_agent_marginal_cost = 0     # the marginal cost of vending
    num_consumers = 2
    consumer_total_money_per_turn = 25000
    consumers_arr = []

    def __init__(self, envTimesteps:int, numAgents: int):
        """
        Initialises the class
        """
        self.environment_starter_timesteps = envTimesteps
        
        self.policy_agents = []
        for i in range(numAgents):
            self.policy_agents.append(AgentObject())
        
        for i in range(self.num_consumers):
            self.consumers_arr.append(ConsumerObject())

        self.observation_space = []
        self.agents = [numAgents]

        arr = np.full(numAgents, 200) # creates an array full of 200's shaped [200,200,200], with numAgents number of element


        self.action_space = spaces.MultiDiscrete(arr)

        # the observation space is a nAgents by nActions array of float32 numbers between -99-99
        # also contains the static value for marginal cost and wholesale price
        self.observation_space = spaces.Box(low=-100,high=100, shape=(numAgents, 4), dtype=np.float32)

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
        # agent.state is the percentage price diff from the 
        # wholesale price
        agent.state = action - 100
        agentBaseVendingPriceAdjust = self.env_wholesale_price * (agent.state / 100)
        baseAgentVendingPrice = self.env_wholesale_price + agentBaseVendingPriceAdjust
        #agentMarginalCostAddedVendingPrice = agentBaseVendingPriceAdjust + self.env_agent_marginal_cost
        agent.vendingPrice = max(baseAgentVendingPrice, 1)
        # print(f'agent vending price was {agent.vendingPrice}')

    def step_agent(self, agent):
        # if agent.state > 0:
        #     reward = 1
        # elif agent.state == 0:
        #     reward = 0
        # else:
        #     reward = -1
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

        for i, agent in enumerate(self.policy_agents):
            agent.state, agent.reward, agent.done, agent.info = self.step_agent(agent)

        for agent in self.policy_agents:
            obs_arr.append(self._get_obs(agent))
            reward_arr.append(self._get_reward(agent))
            done_arr.append(self._get_done(agent))
            info_arr['n'].append(self._get_info(agent))

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
            tmpObsArray.append(partialObservationResult)

        concatObsArray = np.array(tmpObsArray).astype(np.float32) 
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

        while consumer.money > 0:            
            if agents_arr[lowestPriceAgnetIndex].vendingPrice < consumer.money:
                agents_arr[lowestPriceAgnetIndex].reward += agents_arr[lowestPriceAgnetIndex].vendingPrice
                consumer.money -= agents_arr[lowestPriceAgnetIndex].vendingPrice
                consumer.total_consumed += 1
            else:
                # set the consumer's money to 0 if the vend price
                # is less than the remaining money (stops infinite loop)
                consumer.money = 0



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

    def _get_info(self, agent):
        """
            accepts an Agent, and returns its info object
        """
        return agent.info


    def render(self) -> None:
        """
        Does nothing, the environment is fully headless
        """
        pass

    def reset(self) -> float:
        """
        Sets the application to its initial conditions

        Sets state to a random float between negative 100 to  positive 100
        """
        obs_arr =[]
        for i in range(len(self.policy_agents)):
            self.policy_agents[i].state = np.array(
                                    self.get_agent_default_observation_array(), 
                                     dtype=np.float32)
            obs_arr.append(self.policy_agents[i].state)
            self.policy_agents[i].reward = 0
            self.policy_agents[i].info = {}
            self.policy_agents[i].done = False
            self.policy_agents[i].vendingPrice = 0
        
        consumerMoneyEach = self.consumer_total_money_per_turn / self.num_consumers
        for i in range(len(self.consumers_arr)):
            self.consumers_arr[i].money = consumerMoneyEach

        self.environment_timesteps = self.environment_starter_timesteps
        
        return np.array(obs_arr).astype(np.float32)

    def get_consumer_demand_schedule(self):
        schedule = [
            [0, 1000],
            [10, 900],
            [20, 800],
            [30, 700],
            [40, 600],
            [50, 500],
            [60, 400],
            [70, 300],
            [80, 200],
            [90, 100],
            [100, 0]
        ]
        return schedule
    
    def get_consumer_quantity_demanded_at_price(self, price):
        dmnd_schedule = self.get_consumer_demand_schedule()
        roundedPrice = round(price, -1)
        for i in dmnd_schedule:
            if i[0] == roundedPrice:
                return i[1]

        return 0
      
        # point_a = dmnd_schedule()[0]
        # point_b = dmnd_schedule()[len(dmnd_schedule)]


        # change_in_x = point_a[0] - point_b[0]
        # change_in_y = point_a[1] - point_b[1]
        # gradient = change_in_y / change_in_x



    def get_agent_default_observation_array(self):
        return [0.0, 0.0, self.env_wholesale_price, self.env_agent_marginal_cost]