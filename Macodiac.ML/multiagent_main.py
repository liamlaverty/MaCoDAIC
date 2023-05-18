import os
import gymnasium as gym
from gymnasium import Env
from multiagentenvironment import TensorboardPriceCallback
from multiagentenvironment import MultiAgentMacodiacEnvironment
from stable_baselines3.common.evaluation import evaluate_policy
from stable_baselines3.common.env_checker import check_env
import numpy as np
from stable_baselines3 import PPO


class MultiagentMain():
    isRunning = False

    def __init__(self, mode):
        """
        init the class
        """

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

        self.mode = mode


        filePath = os.path.join('Macodiac.ML', 'training_multiagent','results_quantity_setter')
        self.log_path =  os.path.join(filePath,'Logs')
        self.save_path =  os.path.join(filePath,'saved_models', self.mode)
        self.save_path_intermittent =  os.path.join(filePath,'saved_models', 'intermittent_saved_models')

        self.numEpisodes = 20
        self.envTimesteps = 25

        if self.mode == 'MONOPOLY':
            self.numAgents = 1
            self.numTrainingIterations = 20_000
        elif self.mode == 'DUOPOLY':
            self.numAgents = 2
            self.numTrainingIterations = 30_000
        elif self.mode == 'OLIGOPOLY':
            self.numAgents = 5
            self.numTrainingIterations = 50_000  
        elif self.mode == 'PERFECT_COMP':
            self.numAgents = 10
            self.numTrainingIterations = 150_000
        else:
            raise ValueError(f'self.mode [{self.mode}] was not in mode options list [{self.__MODE_OPTIONS__}]')
        
        if self.numAgents == 0 or self.numTrainingIterations == 0:
            raise ValueError('both numAgents and numTrainingItterations must be above 0')

        self.env = MultiAgentMacodiacEnvironment(envTimesteps=self.envTimesteps, numAgents=self.numAgents)
        check_env(self.env)
        

    def Run(self):
        """
        Runs the project
        """
        if self.__MODE_RANDOMSAMPLE__:
            self.run_multiagent_project_with_rand_test(self.env, self.numEpisodes)

        model = self.create_model(self.env, self.log_path)

        if self.__MODE_LOADMODEL__:
            model = self.load_model(self.env, model, self.save_path)

        if self.__MODE_TRAINMODEL__: 

            model = self.train_model(model,
                                     self.numTrainingIterations, 
                                     self.save_path_intermittent,
                                     self.mode)
            self.save_model(model, self.save_path)

        if not self.__MODE_RANDOMSAMPLE__:
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
                #print(f'iterator:{iterator}')
                action_arr = env.action_space.sample()

                # action_arr = []
                # for i in range(self.numAgents * 2):
                #     action_arr.append(11)

                print(f'action for agents:\t{action_arr}')
                
                obs_arr, reward, isDone, info_arr = env.step(action_arr)
                
                agent_scores.append(reward)

                # print(f'rewards for agents:\t{reward}')
                # print(f'obs for agents:\t{obs_arr}')

                info_arr = info_arr['n']
                print(f'px for agents:\t{info_arr}')
                quantitySold = 0
                moneySales = 0
                for i, agentInfo in enumerate(info_arr):
                    agent_sales = info_arr[i]['sold']
                    agent_price = info_arr[i]['price']
                    agent_sales_in_money = agent_sales * agent_price
                    moneySales += agent_sales_in_money
                    quantitySold += agent_sales
                    
                print(f'a_vending/quantity_sold_count: {quantitySold} at cost [{moneySales}]/[{env.peek_env_consumer_money()}. Consumer money per turn:{env.peek_env_consumer_money_each()}]')
                if moneySales > env.peek_env_consumer_money():
                    print(f'Money sales of [{moneySales}]/[{env.peek_env_consumer_money()}] were too high. Consumer money per turn:{env.peek_env_consumer_money_each()}')
                    return # raise Exception(f'Money sales of [{moneySales}]/[{env.peek_env_consumer_money()}] were too high. Consumer money per turn:{env.peek_env_consumer_money_each()}')


                if done:
                    print(f'is done')
                done = isDone
            print(f'Episode:{episode} | \nAggregate agent scores:(Sum:{sum(agent_scores)})\n MeanAvg agent scores:({np.mean(agent_scores)})')
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

            print(f'Episode:\t{episode} \t| Score:\t{score} \t\t| RunningAvg: {round(runningAvg, 2)}')
        env.close()


    def create_model(self, env: MultiAgentMacodiacEnvironment, log_path: str):
        model = PPO('MlpPolicy', env, verbose=1, tensorboard_log=log_path, device="cpu")
        return model


    def train_model(self, model, numTimesteps: int, savePath:str, saveName: str):
        """
        Trains a model with the number of iterations in 
        numtimesteps. Creates a n intermediate save every 1m iterations

        @param model: The model to train. The model must have been instantiated
        @param numTimesteps: the number of training iterations
        """
        
        saveEveryNSteps = 1_000_000
        
        if numTimesteps < saveEveryNSteps:
            model.learn(total_timesteps=numTimesteps,
                        callback=TensorboardPriceCallback(), 
                        tb_log_name=saveName)

        else:
            rangeUpper = int(numTimesteps / saveEveryNSteps)
            for i in range(1,rangeUpper+1):
                model.learn(total_timesteps=saveEveryNSteps,
                            callback=TensorboardPriceCallback(),
                            tb_log_name=saveName)
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





__MODE_OPTIONS__ = ['MONOPOLY', 'OLIGOPOLY',  'DUOPOLY', 'PERFECT_COMP']
for mode in __MODE_OPTIONS__:
    main = MultiagentMain(mode)
    main.Run()