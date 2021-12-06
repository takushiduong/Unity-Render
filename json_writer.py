import json
import numpy as np
from IPython import embed

'''
this script is used to generate a test file for unity rendering
'''
data = {}
data['num_objects'] = 6
data['geom_types'] = [0, 0, 1, 1, 2, 2]
#process shapes
shapes = []
for i in range(data['num_objects']):
    if(data['geom_types'][i] == 0):
        radius = np.random.uniform(0.005, 0.01)
        shape = np.array([radius,radius, radius]) # for the sphere
    elif(data['geom_types'][i] == 1):
        shape = np.random.uniform([0.005,0.005,0.005], [0.01,0.01,0.01]) # for the box
    elif(data['geom_types'][i] == 2):
        radius = np.random.uniform(0.005, 0.01)
        size = np.random.uniform(0.01,0.02)
        shape = np.array([radius, radius, size])
    else:
        raise NotImplementedError
    print(shape)
    shapes.append(shape)
shapes = np.array(shapes).reshape(-1)
data['shapes'] = shapes.tolist()
#process motions
motions_all = []
for i in range(data['num_objects']):
    motions = np.zeros((100, 7))
    motions[:, 3] = 1
    start_pos = np.random.uniform([-0.2,0.1,-0.2], [0.2,0.1,0.2])
    end_pos = np.random.uniform([-0.2,0.1,-0.2], [0.2,0.1,0.2])
    for j in range(100):
        motions[j, 0:3] = start_pos + (j/100) * (end_pos - start_pos)
    motions = motions.reshape(-1)
    motions_all.append(motions)
motions_all = np.array(motions_all).reshape(-1).tolist()
data['motions'] = motions_all




embed()


with open('./Assets/Assets-main/motions/test.txt', 'w') as f:
    json.dump(data, f)