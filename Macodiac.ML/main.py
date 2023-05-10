import os



class Main():
    isRunning = False

    def __init__(self):
        """
        init the class
        """

    def Run():
        """
        Runs the class
        """
    isRunning = True
    iteraton = 0
    while isRunning:
        print(f'Running loop:{iteraton}')
        iteraton+=1

        if iteraton >= 100:
            isRunning = False
            break



main = Main()
main.Run()