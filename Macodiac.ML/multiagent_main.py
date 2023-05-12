import os
import gymnasium as gym
from gymnasium import Env
from environment import MacodiacEnvironment
from multiagentenvironment import MultiAgentMacodiacEnvironment
from stable_baselines3.common.evaluation import evaluate_policy
from stable_baselines3.common.env_checker import check_env

import numpy as np



from stable_baselines3 import PPO


class MultiagentMain():
    isRunning = False

    def __init__(self):
        """
        init the class
        """
        filePath = os.path.join('Macodiac.ML', 'training_multiagent','results')
        self.log_path =  os.path.join(filePath,'Logs')
        self.save_path =  os.path.join(filePath,'saved_models', 'model')
        self.save_path_intermittent =  os.path.join(filePath,'saved_models', 'intermittent_saved_models')
        self.numTrainingIterations = 5
        self.numEpisodes = 5
        self.numAgents = 2

        self.env = MultiAgentMacodiacEnvironment(envTimesteps=15, numAgents=self.numAgents)
        check_env(self.env)



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
        self.__MODE_RANDOMSAMPLE__ = False


    def Run(self):
        """
        Runs the project
        """


        model = self.create_model(self.env, self.log_path)

        if self.__MODE_LOADMODEL__:
            model = self.load_model(self.env, model, self.save_path)

        if self.__MODE_TRAINMODEL__: 
            model = self.train_model(model,
                                     self.numTrainingIterations, self.save_path_intermittent)
            self.save_model(model, self.save_path)

        if self.__MODE_RANDOMSAMPLE__:
            self.run_multiagent_project_with_rand_test(self.env, 5)
        else:
            self.run_project(self.env, self.numEpisodes, model)
            self.policy_evaluation(model, self.env, self.numEpisodes)


    def run_multiagent_project_with_rand_test(self, env:MultiAgentMacodiacEnvironment, numEpisodes: int):
        """
        Runs the project with random sampling, using the multiagent env
        """

        for episode in range(numEpisodes):
            obs = env.reset()
            done = False
            score = 0
            agent_scores = []
            iterator = 0
            while not done:
                #env.render()
                iterator+=1
                print(f'iterator:{iterator}')
                action_arr = []
                for i in range(len(env.policy_agents)):
                    actionForAgent = env.action_space[i].sample()
                    action_arr.append(actionForAgent)
                
                print(f'action for agents:\t{action_arr}')
                
                obs_arr, reward_arr, done_arr, isTerminal, info_arr = env.step(action_arr)
                
                # for i, reward in enumerate(reward_arr):
                #     print(f'reward is {reward}')

                agent_scores.append(sum(reward_arr))

                print(f'rewards for agents:\t{reward_arr}')

                if any(done_arr):
                    isTerminal = True

                done = isTerminal
            print(f'Episode:{episode} | Aggregate agent scores:(Sum:{sum(agent_scores)})')
        env.close()


    def run_project_with_rand_test(self, env:MultiAgentMacodiacEnvironment, numEpisodes:int):
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
            print(f'Episode:{episode}  | Score:{score}')
        env.close()


    def run_project(self, env:MultiAgentMacodiacEnvironment, numEpisodes: int, model):
        """
        Runs the project with an actual model, instead of random sampling
        of a model

        @param env: The environment to run this project with
        @param numEpisodes: the count of episodes to run the environment for
        """
        scores = []
        for episode in range(numEpisodes):
            obs = env.reset()
            done = False
            score = 0
            while not done:
                #env.render()
                action, _discard = model.predict(obs)
                obs, reward, isTerminal, info = env.step(action)
                score += reward
                done = isTerminal
            scores.append(score)

            runningAvg = np.mean(scores)

            print(f'Episode:{episode} \t| Score:{score} \t\t| RunningAvg: {round(runningAvg, 2)}')
        env.close()


    def create_model(self, env: MultiAgentMacodiacEnvironment, log_path: str):
        model = PPO('MlpPolicy', env, verbose=1, tensorboard_log=log_path)
        return model


    def train_model(self, model, numTimesteps: int, savePath:str):
        """
        Trains a model with the number of iterations in 
        numtimesteps. Creates a n intermediate save every 1m iterations

        @param model: The model to train. The model must have been instantiated
        @param numTimesteps: the number of training iterations
        """
        
        saveEveryNSteps = 1_000_000
        
        if numTimesteps < saveEveryNSteps:
            model.learn(total_timesteps=numTimesteps)

        else:
            rangeUpper = int(numTimesteps / saveEveryNSteps)
            for i in range(1,rangeUpper+1):
                model.learn(total_timesteps=saveEveryNSteps)
                model.save(os.path.join(savePath, f'interim-{i}'))

        return model

    def policy_evaluation(self, model, env: MultiAgentMacodiacEnvironment, numEpisodes:int=50):
        """
        Prints a policy evaluation, including the mean episode reward
        and the standard deviation

        @param model:       The model to be evaluated
        @param env:         The environment to evaluate the model against
        @param numEpisodes: The count of episodes to evaluate against        
        """
        print('\nevalResult:(mean episode reward, standard deviation)')
        print(f'evalResult:{evaluate_policy(model, env, n_eval_episodes=numEpisodes)}\n')



    def save_model(self, model, modelPath):
        """
        Saves a model to a given path

        @param model:       The model to save
        @param modelPath:   The path to save to
        """
        model.save(modelPath)    
    
    
    def load_model(self, env: MultiAgentMacodiacEnvironment, model, modelPath: str):
        """
        Saves a model to a given path

        @param model:       The model to save
        @param modelPath:   The modelPath to save to
        """
        model = PPO.load(modelPath, env=env)
        return model


main = MultiagentMain()
main.Run()