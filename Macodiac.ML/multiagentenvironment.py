import gymnasium as gym
from gymnasium import Env
# use `gym.spaces` here, even though we're using `gymnasium`
# https://stackoverflow.com/questions/75108957/assertionerror-the-algorithm-only-supports-class-gym-spaces-box-box-as-acti
from gym import spaces
import numpy as np
import random


class MultiAgentMacodiacEnvironment(Env):
    """
    Builds a profit maximising agent environment, supporting
    up to n_agent agents
    """
    state = 0
    environment_timesteps = 1000

    def __init__(self, envTimesteps:int, numAgents: int):
        """
        Initialises the class
        """
        self.environment_timesteps = envTimesteps
        self.action_space = spaces.Discrete(3)    
        self.observation_space = spaces.Box(low=np.array([0]), high=np.array([100]))
        
        self.policy_agents = [numAgents]
        
        self.reset()


        
        print('-- ENV SETTINGS --')
        print(self.observation_space)
        print(self.observation_space.sample())
        print(self.action_space)
        print(self.action_space.sample())
        print(self.environment_timesteps)
        print('-- ENV SETTINGS --')


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
        info_arr = [{'n': []}]
        
        agent_arr = self.policy_agents

        for i, agent in enumerate(agent_arr):
            self.set_action(action_arr[i], agent, self.action_space[i])

        self.world.step()

        for agent in self.policy_agents:
            obs_arr.append(self._get_obs(agent))
            reward_arr.append(self._get_reward(agent))
            done_arr.append(self._get_done(agent))
            info_arr.append(self._get_info(agent))

        if self.environment_timesteps <= 0:
            isTerminal = True
        else:
            isTerminal = False

        return obs_arr, reward_arr, done_arr, isTerminal, info_arr

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
        self.state = np.array([0 + random.randint(-100,100)]).astype(float)
        self.environment_timesteps = 1000
        return self.state
        