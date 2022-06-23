# undergraduate_dissertation

ABSTRACT
This paper presents the development of a Deep Reinforcement Learning(DRL) agent and a 3-dimensional imperfect information game environment designed by myself for the single agent to interact with. The DRL agent is trained with a Reinforcement Learning(RL) algorithm known as Proximal Policy Optimization(PPO) in a recurrent neural network known as Long Short-Term Memory(LSTM) where Intrinsic Curiosity Module(ICM) is implemented to help the agent to cope with
the sparse rewards environment. The agent is trained with Curriculum Learning(CL) and Transfer Learning(TL), both of which are popular deep learning approaches along with environment parameter randomization to improve agent robustness. Robustness refers to the generalising capability or generality of the agent in a complex game environment. The model with best training results is used to control the agent for testing. Robustness is then quantitatively evaluated based on testing results, the agent behaviour is qualitatively studied based on human observations and discussed at length. This paper then concludes future work approaches for possible better testing results as well as a reflection on project aim, project workflow management, biggest challenge encountered and the lessons learned throughout this project.

EXPERIMENT
The agent is trained over 22 different consecutive "sessions" in a total of around 76 hours. Each session is "transferred" from one to another and within each session the task is broken down into multiple sub-tasks to create a "curriculum" of easy to difficult task for the agent.

trained models : https://drive.google.com/drive/folders/1IUZj9bgGUQl8qE12p6nvYIIVIJY1BU29?usp=sharing
