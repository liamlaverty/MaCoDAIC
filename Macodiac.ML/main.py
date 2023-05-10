import os
from environment import MacodiacEnvironment


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

        self.run_project()

    def run_project_with_rand_test(self):
        """
        Runs the project with random sampling, instead
        of a model
        """
        pass

    def run_project(self):
        """
        Runs the project with a prebuilt model
        """
        pass

    def create_model(self, env: MacodiacEnvironment, log_path: str):
        print('create_model not implemented')
        pass

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