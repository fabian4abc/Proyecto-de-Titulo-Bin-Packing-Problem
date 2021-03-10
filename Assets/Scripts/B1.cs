using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Diagnostics;
using UnityEngine.Analytics;



public class B1 : MonoBehaviour
{
    private float posX;
    private float posY;
    private float posZ;
    private float pol;
    private float range_duales;
    private float posY0;

    private float totDuales;

    private int num_figuras;
    private bool did_close;
    private int iteracion;
    private int cont5;
    private int[] cpol;

    private float xrnd;
    private float xx;
    public int ite;
    private int Occupacion;
    private int Occ1;
    private float y;

    private int accion;
    private int real_counter;
    private int esp;
    private int counter;
    public bool bol;
    public bool Dual;

    private string body;

    private GameObject obj;
    public GameObject pol1;
    public GameObject pol2;
    public GameObject pol3;
    public GameObject pol4;
    private int numGO;

    private GameObject current;
    private GameObject gup;
    private GameObject gdown;
    private GameObject gright;
    private GameObject gleft;
    private GameObject gbox;

    ArrayList poligonos = new ArrayList();
    ArrayList duales = new ArrayList();
    ArrayList cantPol = new ArrayList();
    ArrayList nombres = new ArrayList();
    List<GameObject> Polx = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

        //Actualizar el número
        numGO = 4;
        cpol = new int[numGO];

        gup = GameObject.Find("Up");
        gdown = GameObject.Find("Down");
        gright = GameObject.Find("Right");
        gleft = GameObject.Find("Left");
        gbox = GameObject.Find("Box");
        bol = false;

        posX = gup.transform.position.x;
        posY0 = gup.transform.position.y;
        posZ = gup.transform.position.z;
        Physics2D.gravity = new Vector2(0, -9.8f);
        real_counter = 0;

        num_figuras = 0;
        did_close = false;
        accion = 0;
        iteracion = 0;
        cont5 = 0;

        esp = 0;
        Occupacion = 0;
        Occ1 = 0;

        xrnd = -18f;
        xx = -30f;
        y = -20f;
        pol = 0;

        Polx.Add(pol1);
        Polx.Add(pol2);
        Polx.Add(pol3);
        Polx.Add(pol4);

        string path = "Assets/Resources/datos.txt";
        StreamWriter writer = new StreamWriter(path, false);
        string path3 = "Assets/Resources/duales.txt";
        StreamWriter writer3 = new StreamWriter(path3, false);
        for (int i = 1; i < numGO+1; i++)
        {
            writer3.WriteLine(1);
        }
        writer3.WriteLine(-1);
        writer3.Close();
        print("Iteración " + iteracion.ToString());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!did_close)
        {
            if (real_counter++ % 10 == 0)
            {
                Instanciar();
            }
        }
        if (Porcentaje())
        {
            if (accion == 0 || accion == 2)
            {
                Cerrar();
            }
            else if (accion == 1)
            {
                Subir();
            }
            else if (accion == 3 || accion == 5)
            {
                bol = false;
                if (esp++ == 105)
                {
                    BorrarAfuera();
                }
            }
            else if (accion == 4)
            {

                bol = true;

                if (real_counter++ % 20 == 0)
                {
                    FiguraTop(counter % numGO);
                    counter++;
                }
            }
            else if (accion == 6)
            {
                    Imprimir();
            }
            else if (accion == 7)
            {

                if (iteracion < ite)
                {

                    if (ReadString())
                    {
                        if (Dual) { ChangeDuales(); }
                        iteracion++;
                        Borrar();
                        print("Iteración " + iteracion.ToString());
                    }
                }
                else
                {
                    print(Time.unscaledTime);
                    accion++;
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
        }
    }

    public void ChangeDuales()
    {
        int c = 0;
        for (int i = 0; i < duales.Count; i++)
        {
            if ((float)duales[i] != 0)
            {
                c++;
            }
        }

        if (c <= 2)
        {
            for (int i = 0; i < duales.Count; i++)
            {
                if ((float)duales[i] != 0)
                {
                    duales[i] = (float)0.6 / c;
                }
                else
                {
                    duales[i] = (float)0.4 / (numGO - c);
                }
            }

            totDuales = 0f;
            for (int i = 0; i < numGO; i++)
            {
                totDuales = totDuales + (float)duales[i];
            }
        }
    }
    public void Instanciar()
    {
        gup.transform.position = new Vector3(posX, posY0 + 30f, posZ);
        xrnd = xrnd + 18;
        if (xrnd > 18)
        {
            xrnd = -18f;
        }
        // elegir obj aleatorio y su pos
        if (iteracion == 0)
        {
            obj = Polx[cont5 % numGO];
            pol = cont5 % numGO;
            cont5++;
        }
        else
        {
            range_duales = UnityEngine.Random.Range(0, totDuales);
            float acumulado = 0f;
            for (int i = 0; i < numGO; i++)
            {
                acumulado += (float)duales[i];
                if (range_duales <= acumulado)
                {
                    obj = Polx[i];
                    pol = i;
                    break;
                }
            }
        }

        if (num_figuras % 6 == 0)
        {
            y = y + 5f;
        }
        var clone = Instantiate(obj, new Vector3(xrnd, posY0 + Math.Min(20f, y), 0f), Quaternion.identity);

        clone.transform.Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
        clone.name = "Pol_(" + (pol) + ")" + num_figuras;
        nombres.Add("Pol_(" + (pol).ToString() + ")" + num_figuras.ToString());
        num_figuras++;
        poligonos.Add((pol));
        Occupacion = Occupacion + (int)clone.transform.GetChild(0).GetComponent<Rigidbody2D>().mass;

    }
    public bool Porcentaje()
    {
        return Occupacion > (59 * 40);
    }

    public void Cerrar()
    {
        did_close = true;
        posY = gup.transform.position.y;
        gup.transform.position = new Vector3(posX, posY - 0.1f, posZ);

        // Cuando termina de cerrar
        if (gup.transform.position.y <= posY0 - 0.5f)
        {
            gup.transform.position = new Vector3(posX, posY0, posZ);
            accion++;
        }
    }

    public void Subir()
    {
        posY = gup.transform.position.y;
        gup.transform.position = new Vector3(posX, posY + 0.1f, posZ);
        if (gup.transform.position.y >= posY0 + 1f)
        {
            accion++;
        }
    }

    public void BorrarAfuera()
    {
        Physics2D.gravity = new Vector2(0, -9.8f);
        object[] obj1 = GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (object o in obj1)
        {
            GameObject g = (GameObject)o;
            if (g.name.Substring(0, 2) == "Po")
            {
                GameObject child = g.transform.GetChild(0).gameObject;
                if (child.transform.position.y > gup.transform.position.y ||
                    child.transform.position.y < gdown.transform.position.y ||
                    child.transform.position.x > gright.transform.position.x ||
                    child.transform.position.x < gleft.transform.position.x)
                {
                    Destroy(g);
                }
            }
        }

        int Occ2 = Ocupado();
        if (accion == 3)
        {
            ScreenShot();
            Occ1 = Occ2;
            Guardar();

        }
        else if (Occ2 >= Occ1)
        {
            ScreenShot();
            Guardar();
        }
        accion++;
        print("Ocupación: " + Math.Max(Occ1, Occ2));
    }

    public void Guardar()
    {
        for (int i = 0; i < cpol.Length; i++)
        {
            cpol[i] = 0;
        }

        object[] obj1 = GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (object o in obj1)
        {
            GameObject g = (GameObject)o;
            if (g.name.Substring(0, 2) == "Po")
            {
                GameObject child = g.transform.GetChild(0).gameObject;

                cpol[int.Parse(g.name.Substring(5, 1))]++;

            }
        }
    }

    public void Imprimir()
    {

        for (int i = 0; i < cpol.Length; i++)
        {
            cantPol.Add(cpol[i]);
        }

        string path = "Assets/Resources/datos.txt";
        StreamWriter writer = new StreamWriter(path, true);

        if (iteracion == 0)
        {
            string header = "Pol0\tPol1\tPol2\tPol3";
            writer.WriteLine(header);
        }

        //Imprimo el número de figuras en cada iteración
        for (int j = 0; j < numGO; j++)
        {
            if (j == numGO - 1)
            {
                body = body + cantPol[j + (numGO * iteracion)].ToString();
            }
            else
            {
                body = body + cantPol[j + (numGO * iteracion)].ToString() + "\t";
            }
        }
        body = body + "\n";

        writer.WriteLine(body);
        writer.Close();

        string path2 = "Assets/Resources/iteraciones.txt";
        StreamWriter writer2 = new StreamWriter(path2, false);
        string header2 = iteracion.ToString();

        writer2.WriteLine("Iteraciones");
        writer2.WriteLine(header2);
        writer2.Close();

        Correr();
        Ocupado();
        accion++;


    }

    public void Correr()
    {
        //Run the .exe file
        System.Diagnostics.Process MP = new System.Diagnostics.Process();
        MP.StartInfo.RedirectStandardInput = true;
        MP.StartInfo.RedirectStandardOutput = true;
        MP.StartInfo.UseShellExecute = false;
        MP.StartInfo.CreateNoWindow = true;
        MP.StartInfo.WorkingDirectory = Application.dataPath + "/Resources";
        MP.StartInfo.FileName = "cmd.exe";
        MP.Start();

        MP.StandardInput.WriteLine("activate");
        MP.StandardInput.Flush();

        MP.StandardInput.WriteLine("python MP.py");
        MP.StandardInput.Flush();
        MP.Close();

    }

    public void Borrar()
    {
        object[] obj1 = GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (object o in obj1)
        {
            GameObject g = (GameObject)o;
            if (g.name.Substring(0, 2) == "Po") { Destroy(g); }
        }

        num_figuras = 0;
        did_close = false;
        accion = 0;
        real_counter = 0;
        counter = 0;
        esp = 0;
        y = -20f;
        Occupacion = 0;
        xrnd = -18f;
        body = "";
        pol = 0f;
        nombres.Clear();
        poligonos.Clear();
        Physics2D.gravity = new Vector2(0, -9.8f);
        //=============================================================================
    }

    // captura las variables duales del problema maestro
    public bool ReadString()
    {
        string path = "Assets/Resources/duales.txt";

        //Read the text from directly from the test.txt file
        duales.Clear();
        StreamReader reader = new StreamReader(path);
        for (int i = 1; i < numGO+1; i++)
        {
            duales.Add(float.Parse(reader.ReadLine(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
        }

        int check = int.Parse(reader.ReadLine());
        reader.Close();

        //capturo el valor total de las variables duales

        totDuales = 0f;
        for (int i = 0; i < numGO; i++)
        {
            totDuales = totDuales + (float)duales[i];
        }

        return check == iteracion;
    }


    public void ScreenShot()
    {
        string path1 = "Assets/Resources/Images/SS" + iteracion.ToString() + ".png";
        ScreenCapture.CaptureScreenshot(path1);
    }

    public int Ocupado()
    {
        int Occ = 0;
        object[] obj1 = GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (object o in obj1)
        {
            GameObject g = (GameObject)o;
            if (g.name.Substring(0, 2) == "Po")
            {
                Occ = Occ + (int)g.transform.GetChild(0).GetComponent<Rigidbody2D>().mass;
            }
        }
        return Occ;
    }

    public void FiguraTop(int cont) {
        if (Ocupado() <= (59 * 40 * 0.65)) {
            xx = xx + 15;
            if(xx > 15) { xx = -15; }

            var b = Instantiate(Polx[cont], new Vector3(xx, 10f, 0f), Quaternion.identity);
                b.name = "Pol_("+cont+")" + (num_figuras + counter);
            
        } else
        {
            accion++;
            esp = 0;
        }
    }
}
