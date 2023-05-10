import os
import gymnasium as gym
from gymnasium import Env
from environment import MacodiacEnvironment
from stable_baselines3.common.evaluation import evaluate_policy




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


        # set to true if you want to load an existing model
        # model loading happens first, then training
        # NOTES: 
        #   if loadmodel is set to false, and trainmodel is set to true, 
        #   the currently saved model is overwritten
        self.__MODE_LOADMODEL__ = False

        # set to true if you want to train and then save the model
        self.__MODE_TRAINMODEL__ = True

        # set to true to use the randomsample mode for testing, 
        # rather than the model version
        self.__MODE_RANDOMSAMPLE__ = True


    def Run(self):
        """
        Runs the project
        """
        model = self.create_model(self.env, self.log_path)

        if self.__MODE_LOADMODEL__:
            model = self.load_model(model, self.save_path)

        if self.__MODE_TRAINMODEL__: 
            model = self.train_model(model,
                                     self.numTrainingIterations)

        self.save_model(model, self.save_path)

        if self.__MODE_RANDOMSAMPLE__:
            self.run_project_with_rand_test(self.env, self.numEpisodes)
        else:
            self.run_project(self.env, self.numEpisodes)


    def run_project_with_rand_test(self, env:MacodiacEnvironment, numEpisodes:int):
        """
        Runs the project with random sampling, instead
        of a model

        @param env: The environment to run this project with
        @param numEpisodes: the count of episodes to run the environment for
        """
        for episode in range(numEpisodes):
            obs = env.reset()
            done = False
            score = 0
            while not done:
                #env.render()
                action = env.action_space.sample()
                obs, reward, isTerminal, info = env.step(action)
                score += reward
                done = isTerminal
            print(f'Episode:{episode} | Score:{score}')
        env.close()


    def run_project(self, env:MacodiacEnvironment, numEpisodes: int, model):
        """
        Runs the project with an actual model, instead of random sampling
        of a model

        @param env: The environment to run this project with
        @param numEpisodes: the count of episodes to run the environment for
        """
        for episode in range(numEpisodes):
            obs = env.reset()
            done = False
            score = 0
            while not done:
                #env.render()
                action, _discard = model.preduct(obs)
                obs, reward, isTerminal, info = env.step(action)
                score += reward

                done = isTerminal
            print(f'Episode:{episode} | Score:{score}')
        env.close()


    def create_model(self, env: MacodiacEnvironment, log_path: str):
        model = PPO('MlpPolicy', env, verbose=1, tensorboard_log=log_path)
        return model


    def train_model(self, model, numTimesteps: int):
        """
        Trains a model with the number of iterations in 
        numtimesteps

        @param model: The model to train. The model must have been instantiated
        @param numTimesteps: the number of training iterations
        """
        model.learn(total_timesteps=numTimesteps)
        return model

    def policy_evaluation(model, env: MacodiacEnvironment, numEpisodes:int=50):
        print('\nevalResult:(mean episode reward, standard deviation)')
        print(f'evalResult:{evaluate_policy(model, env, n_eval_episodes=numEpisodes)}\n')



    def save_model(self, model, modelPath):
        """
        Saves a model to a given path

        @param model:       The model to save
        @param path:        The path to save to
        """
        model.save(modelPath)    
    
    
    def load_model(self, model, modelPath):
        """
        Saves a model to a given path

        @param model:       The model to save
        @param path:        The path to save to
        """
        model.save(modelPath)

main = Main()
main.Run()