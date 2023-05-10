



class MacodiacEnvironment:
    """
    Builds a profit maximising agent
    """
    state = 0
    environment_timesteps = 1000

    def __init__(self):
        """
        Initialises the class
        """
        pass

    def step(self, action):
        """
        Processes an action for an agent
        """

        self.state += action - 1
        self.environment_timesteps -=1

        

        pass

    def render(self) -> None:
        """
        Does nothing, the environment is fully headless
        """
        pass

    def reset(self) -> float:
        """
        Sets the application to its initial conditions
        """

        pass