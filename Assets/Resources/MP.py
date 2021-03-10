# -*- coding: utf-8 -*-
#Immportar librerias

from gurobipy import * 
import pandas as pd           
#Importar gurobi

m= Model("MasterProblem")

#Definición de conjuntos
P = []    #Conjunto de poligonos
J = []      #Conjunto de patrones

#Definición de parámetros

a = {}  #Cantidad de poligonos p in P que tiene el patrin j in J
b = {}  #Demanda del poligono p in P
e = {}  #Cantidad de patrones
f = {}


c = pd.read_csv("datos.txt",sep="\t")
d=pd.read_csv("demanda.txt",sep="\t")
f=pd.read_csv("iteraciones.txt")
g=f.to_dict()
c['Pol1'].to_dict()


for i in range(0,g["Iteraciones"][0]+1):
    J.append(i)

for i in J:
    e[i]=i

for i in c:
    a[c[i].name]=c[i].to_dict()
    P.append(i)
        
for i in d:
    b[d[i].name]=int(d[i].values)  
    

x = {i:m.addVar(vtype = GRB.CONTINUOUS ,name="x_"+str(i))for i in J}
#CONTINUOUS
#INTEGER
for i in P:
	m.addConstr(quicksum(x[j]*a[i][j] for j in J) >= b[i], "Demanda"+str(i))
    
m.setObjective(quicksum(x[j] for j in J) , GRB.MINIMIZE)

m.optimize()

#Mostrar los resultados
def imprimirResultados():
    for j in J:
            print("La cantidad de patrones " + str(j) + " es: " +str(x[j].x))

imprimirResultados()

duales = []

for i in P:
    duales.append(m.getConstrByName("Demanda"+str(i)).pi)

f= open("duales.txt","w+")

for i in range(0,len(duales)):
    f.write(str(duales[i]) + "\n")

f.write(str(g["Iteraciones"][0]))
f.close()




