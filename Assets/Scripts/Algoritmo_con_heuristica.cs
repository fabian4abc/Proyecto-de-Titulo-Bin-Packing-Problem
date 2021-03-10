using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using UnityEngine.Analytics;

public class Algoritmo_con_heuristica : MonoBehaviour
{
    //este es el objeto que renderizara unity
    private GameObject obj;

    //estos son los poligonos que se utilizaran en la simulacion
    public GameObject pol1;
    public GameObject pol2;
    public GameObject pol3;
    public GameObject pol4;
    public GameObject pol5;
    public GameObject pol6;
    public GameObject pol7;

    //los datos de un poligono estaran guardados en una clase con el mismo nombre que el de la figura
    public Poligono pol_agregar;

    //booleano para dejar de generar figuras
    public bool generar = true;

    //numero de figuras que se renderizara
    public int n_figuras = 4;

    //contador del ciclo
    public int i = 0;
    
    //ArrayList poligonos_clase = new ArrayList();
    //esta lista contiene las clases de los poligonos con datos como angulos, vertices, etc...
    List<Poligono> poligonos_clase = new List<Poligono>();
    IEnumerable<Poligono> poligonos_clase_ordenados;

    //esta lista tiene los poligonos como gameobject es decir, para renderizarlos en unity
    List<GameObject> Polx = new List<GameObject>();

    //constantes de la heuristica
    public double gamma = 1.5;
    public double factor_ponderacion = 1.0002;


    // Start is called before the first frame update
    void Start()
    {
        //se agregan cada uno de los polignos a la lista
        Polx.Add(pol1);
        Polx.Add(pol2);
        Polx.Add(pol3);
        Polx.Add(pol4);
        Polx.Add(pol5);
        Polx.Add(pol6);
        Polx.Add(pol7);
        //se lee el archivo de texto con la informacion de los poligonos
        string path = "Assets/Resources/angulos.txt";
        StreamReader reader = new StreamReader(path);
        string line = "";
        try
        {
            while ((line = reader.ReadLine()) != null){
                //print(line);
                //se crea un poligono local para luego agregarlo a la lista
                pol_agregar = new Poligono();
                //se separa la informacion del txt y se formatea para la clase
                string[] subs = line.Split(' ');
                //print(subs[0]);
                //print(subs[1]);
                //esta variable permite identificar cual es el nombre del poligono, para separarlo de los angulos
                int posicion_contador = 0;
                //se crea esta variable para darle un largo al arreglo de la cantidad de vertices de cada poligono
                int cantidad_vertices = subs.Length - 1;
                double[] angulos_agregar = new double[cantidad_vertices];
                int contador_vertices = 0;
                foreach(string dato in subs){
                    //si contador es 0 significa que dato es el nombre del poligono
                    if(posicion_contador == 0){
                        pol_agregar.nombre = dato;
                        posicion_contador++;
                    }else{
                        //se agrega cada uno de los angulos leidos en el txt a angulos_agregar
                        angulos_agregar[contador_vertices] = double.Parse(dato);
                        contador_vertices++;
                    }
                }
                //angulos_agregar es un arreglo local el cual ahora se añade a la clase local de poligono
                pol_agregar.angulos = angulos_agregar;
                //ahora el objeto poligono local se agrega a un arreglo global de poligonos
                poligonos_clase.Add(pol_agregar);
            }
        }
        catch (System.Exception e)
        {
            print(e);
        }
        ordenar_poligonos();

    }

    // Update is called once per frame
    void Update()
    {
    obj = Polx[1];
        if(i < 5){
            var clone = Instantiate(obj, new Vector3(1, 2, 0f), Quaternion.identity);
            //clone.transform.Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
            clone.name = "232";   
            //generar = false;
            i++;
            
        }
    }
    //en esta funcion se calculara la heuristica de regularidad de cada poligono
    public void ordenar_poligonos(){
        foreach(Poligono pol_heuristica in poligonos_clase){
            double sum = 0;
            double k = 0;
            double cantidad_vertices_heu = Convert.ToDouble(pol_heuristica.angulos.Length);
            double mejor_angulo = Convert.ToDouble(((cantidad_vertices_heu - 2) * 180)/cantidad_vertices_heu);
            print("mejor angulo es... "+mejor_angulo);
            double k_teo = Math.Abs(Math.Pow(mejor_angulo, gamma) - 90);
            double hr = 0;
            double ht = 0;
            double delta_h = 0;
            //se realiza el calculo de las heuristicas de regularidad
            foreach(double angulo in pol_heuristica.angulos){
                k = Math.Abs(Math.Pow(angulo, gamma) - 90);
                sum = Math.Abs((k * angulo) - 90);
                hr = hr + sum;

                sum = Math.Abs((k_teo * mejor_angulo) - 90);
                ht = ht + sum;
            }
            pol_heuristica.regularidad = Math.Abs(hr - Math.Pow(ht,factor_ponderacion));
        }
        //se ordena la lista de forma ascendente usando el parametro regularidad obtenido por la heuristica
        poligonos_clase_ordenados = poligonos_clase.OrderBy(Poligono => Poligono.regularidad);
    }
}
public class Poligono{
    public double[] angulos;
    public string nombre = "hola! :D";
    public double regularidad;
}




