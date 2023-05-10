import os
import gymnasium as gym
from gymnasium import Env
# use `gym.spaces` here, even though we're using `gymnasium`
# https://stackoverflow.com/questions/75108957/assertionerror-the-algorithm-only-supports-class-gym-spaces-box-box-as-acti
from gym import spaces
import numpy as np
import random
from environment import MacodiacEnvironment


from stable_baselines3 import PPO


class Main():
    isRunning = False

    def __init__(self):
        """
        init the class
        """
        self.log_path =  os.path.join('training', 'results', 'logs', 'shower')
        self.save_path =  os.path.join('training', 'results', 'saved_models', 'shower')
        self.env = MacodiacEnvironment()
        self.numTrainingIterations = 1_000
        self.numEpisodes = 50

        self.__LOADMODEL__ = False
        self.__TRAINMODEL__ = False


    def Run(self):
        """
        Runs the class
        """
        model = self.create_model(self.env, self.log_path)

        if self.__LOADMODEL__:
            model = self.load_model(self.save_path, self.env, model)

        if self.__TRAINMODEL__: 
            model = self.train_model(self.env,
                                     self.numTrainingIterations)

        self.run_project_with_rand_test(self.env, self.numEpisodes)

    def run_project_with_rand_test(self, env:MacodiacEnvironment, numEpisodes:int):
        """
        Runs the project with random sampling, instead
        of a model
        """
        for episode in range(numEpisodes):
            obs = env.reset()
            done = False
            score = 0
            while not done:
                env.render()
                action = env.action_space.sample()
                obs, reward, done, info = env.step(action)
                score += reward
            print(f'Episode:{episode} | Score:{score}')
        env.close()

    def run_project(self, numEpisodes: int):
        """
        Runs the project with a prebuilt model
        """
        pass

    def create_model(self, env: MacodiacEnvironment, log_path: str):
        model = PPO('MlpPolicy', env, verbose=1, tensorboard_log=log_path)
        return model


    def load_model(self, env: MacodiacEnvironment, save_path: str):
        print('load_model not implemented')
        pass

    def train_model(self, env: MacodiacEnvironment, model):
        print('train_model not implemented')
        pass

    def policy_evaluation(self, env: MacodiacEnvironment, numEpisodes:int):
        print ('policy_evaluation not implemented')
        pass

    

main = Main()
main.Run()