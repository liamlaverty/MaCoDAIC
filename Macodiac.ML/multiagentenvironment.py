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

class MultiAgentMacodiacEnvironment(Env):
    """
    Builds a profit maximising agent environment, supporting
    up to n_agent agents
    """
    state = 0
    environment_timesteps = 0
    environment_starter_timesteps = 15

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

        self.action_space = spaces.MultiDiscrete([3, 3])
        self.observation_space = spaces.MultiDiscrete([3, 3])

        # for agent in self.policy_agents:
            #self_action_space.append(spaces.Discrete(3))
            #self_observation_space.append(spaces.Box(low=np.array([0]), high=np.array([100])))
            # self.observation_space.append(spaces.Box(low=-np.inf, high=+np.inf, shape=(5,), dtype=np.float32))
        
        #self.action_space = np.array(self_action_space)
        # self.observation_space = spaces.Box(low=np.array([0, 0, 0]), high=np.array([100, 100, 100]), 
        #                                     shape=(3,3,3),
        #                                     dtype=np.float64)
        self.reset()


        
        print('-- ENV SETTINGS --')
        print(self.observation_space)
        print(self.observation_space[0].sample())
        print(self.action_space)
        print(self.action_space.sample())
        print(self.environment_timesteps)
        print('-- ENV SETTINGS --')


    def set_agent_action(self, action, agent, actionSpace):
        agent.state = action

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
        
        agent_arr = self.policy_agents

        for i, agent in enumerate(agent_arr):
            self.set_agent_action(action_arr[i], agent, self.action_space[i])

        for i, agent in enumerate(agent_arr):
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

        return  np.array(obs_arr), sum(reward_arr), isTerminal,  info_arr


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
            self.policy_agents[i].state = np.array([0 + random.randint(-100,100)]).astype(float)
            obs_arr.append(self.policy_agents[i].state)
            self.policy_agents[i].reward = 0
            self.policy_agents[i].info = {}
            self.policy_agents[i].done = False

        self.environment_timesteps = self.environment_starter_timesteps
        return np.zeros(len(self.policy_agents)).astype(np.int64)
        