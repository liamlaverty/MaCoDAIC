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

class MultiAgentMacodiacEnvironment(Env):
    """
    Builds a profit maximising agent environment, supporting
    up to n_agent agents
    """
    state = 0
    environment_timesteps = 0
    environment_starter_timesteps = 150
    env_wholesale_price = 60        # the price agents pay to purchase goods
    env_agent_marginal_cost = 5     # the marginal operating cost

    def __init__(self, envTimesteps:int, numAgents: int):
        """
        Initialises the class
        """
        self.environment_starter_timesteps = envTimesteps

        self.policy_agents = []
        for i in range(numAgents):
            self.policy_agents.append(AgentObject())
        self.observation_space = []
        self.agents = [numAgents]
        numActions = 3


        self.action_space = spaces.MultiDiscrete([200, 200, 200])

        # the observation space is a nAgents by nActions array of float32 numbers
        # between 0-100
        self.observation_space = spaces.Box(low=-100,high=100, shape=(3, 3), dtype=np.float32)

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
        agentBaseVendingPrice = self.env_wholesale_price * (agent.state / 100)
        agentMarginalCostAddedVendingPrice = agentBaseVendingPrice + self.env_agent_marginal_cost
        agent.vendingPrice = agentMarginalCostAddedVendingPrice
        print(f'agent vending price was {agent.vendingPrice}')

    def step_agent(self, agent):
        if agent.state > 0:
            reward = 1
        elif agent.state == 0:
            reward = 0
        else:
            reward = -1

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

        fakeObsArray = np.array([   [0, 0, 0],
                            [0, 0, 0],
                            [0, 0, 0]]).astype(np.float32) 

        return  fakeObsArray, sum(reward_arr), isTerminal,  info_arr


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
                                    [0.0, 60.0, 5.0], 
                                     dtype=np.float32)
            obs_arr.append(self.policy_agents[i].state)
            self.policy_agents[i].reward = 0
            self.policy_agents[i].info = {}
            self.policy_agents[i].done = False
            self.policy_agents[i].vendingPrice = 0

        self.environment_timesteps = self.environment_starter_timesteps
        
        print(f'obsArr: {obs_arr}')
        return np.array(obs_arr).astype(np.float32)
        
        return np.array([   [0, 0, 0],
                            [0, 0, 0],
                            [0, 0, 0]]).astype(np.float32) 

        