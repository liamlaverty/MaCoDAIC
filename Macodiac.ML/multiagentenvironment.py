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

    def __init__(self, envTimesteps:int, n_agents: int):
        """
        Initialises the class
        """
        self.environment_timesteps = envTimesteps
        self.action_space = spaces.Discrete(3)    
        self.observation_space = spaces.Box(low=np.array([0]), high=np.array([100]))
        self.reset()


        
        print('-- ENV SETTINGS --')
        print(self.observation_space)
        print(self.observation_space.sample())
        print(self.action_space)
        print(self.action_space.sample())
        print(self.environment_timesteps)
        print('-- ENV SETTINGS --')


    def step(self, action):
        """
        Processes an action for an agent
        """
        self.state += action - 1
        self.environment_timesteps -=1


        if self.state > 0:
            reward = 1
        elif self.state == 0:
            reward = 0
        else:
            reward = -1

        if self.environment_timesteps <= 0:
            done = True
        else:
            done = False

        info = {}
        return self.state, reward, done, info

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
        