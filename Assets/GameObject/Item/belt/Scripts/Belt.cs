using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Belt : MonoBehaviour
{

    LineRenderer line;
    List<GameObject> list = new List<GameObject>();

    [SerializeField] Camera camera;

    enum BeltState
    {
        NOLMAL,
        PREPARATION,
        SPIN,
        BREAK,
    }
    BeltState beltState = BeltState.NOLMAL;


    float x = 0;
    float y = 0;

    float rpos = 0;

    float distance = 0;


    Vector3[] poss = new Vector3[100];


    GameObject[] game = new GameObject[100];


    [SerializeField] GameObject p;


    [SerializeField] Gear[] Gears;
    [System.Serializable]
    public class Gear
    {
        public Vector3 pos;
        public Vector3 point1;
        public Vector3 point2;
        public Vector3 point3;
        public Vector3 point4;

        public Vector3 point5;
        public Vector3 point6;
        public float radius = 1;
    }
    int gearsListCount = 0;


    [SerializeField] BeltPoint[] beltPoints;
    [System.Serializable]
   public class BeltPoint
    {
        public GameObject game;
        public Vector3 pos;
        public float moveCountDown;
        public int gearNumber;

        public bool lien = false;


        public float moveCountDownMax;

    }

    // Start is called before the first frame update
    void Start()
    {

        line = GetComponent<LineRenderer>();

        //線の幅を決める
        this.line.startWidth = 0.1f;
        this.line.endWidth = 0.1f;

        //頂点の数を決める

        this.line.positionCount= 0;

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && gearsListCount != 3)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

            //ギアだったらそのまま移動へ
            if (hit2d.collider.gameObject.tag == Common.Gear)
            {

                Gears[gearsListCount].pos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                               camera.ScreenToWorldPoint(Input.mousePosition).y,
                                               0);
                gearsListCount++;

                if (gearsListCount == 3)
                {
                    beltState = BeltState.PREPARATION;
                }
            }
            
        }






        switch (beltState)
        { 
            case BeltState.PREPARATION:
                GearStatus();
                break;

            case BeltState.SPIN:
                Spin();
            break;
        }
        


    }


    void GearStatus()
    {

        for (int i = 0; i < Gears.Length; i++)
        {

            int iNext = i + 1;
            int iPrev = i - 1;

            if (iNext == Gears.Length)
            {
                iNext = 0;
            }

            if(i == 0)
            {
                iPrev = Gears.Length - 1;
            }


            //Next
            x = Mathf.Cos(Mathf.Atan2(Gears[iNext].pos.x - Gears[i].pos.x, Gears[iNext].pos.y - Gears[i].pos.y)) * Gears[i].radius;
            y = Mathf.Sin(Mathf.Atan2(Gears[iNext].pos.x - Gears[i].pos.x, Gears[iNext].pos.y - Gears[i].pos.y)) * Gears[i].radius;
            
            Vector3 nowNextPos = new Vector3(y, x, 0);


            x = Mathf.Cos(90 * Mathf.Deg2Rad) * nowNextPos.x - Mathf.Sin(90 * Mathf.Deg2Rad) * nowNextPos.y;
            y = Mathf.Sin(90 * Mathf.Deg2Rad) * nowNextPos.x + Mathf.Cos(90 * Mathf.Deg2Rad) * nowNextPos.y;

            Gears[i].point1 = new Vector3(x, y, 0);

            x = Mathf.Cos(-90 * Mathf.Deg2Rad) * nowNextPos.x - Mathf.Sin(-90 * Mathf.Deg2Rad) * nowNextPos.y;
            y = Mathf.Sin(-90 * Mathf.Deg2Rad) * nowNextPos.x + Mathf.Cos(-90 * Mathf.Deg2Rad) * nowNextPos.y;

            Gears[i].point2 = new Vector3(x, y, 0);



            //Preve
            x = Mathf.Cos(Mathf.Atan2(Gears[iPrev].pos.x - Gears[i].pos.x, Gears[iPrev].pos.y - Gears[i].pos.y)) * Gears[i].radius;
            y = Mathf.Sin(Mathf.Atan2(Gears[iPrev].pos.x - Gears[i].pos.x, Gears[iPrev].pos.y - Gears[i].pos.y)) * Gears[i].radius;

            Vector3 nowPrevPos = new Vector3(y, x, 0);
            //Debug.Log(Mathf.Atan2(nowPrevPos.x, nowPrevPos.y) * Mathf.Rad2Deg);

            x = Mathf.Cos(90 * Mathf.Deg2Rad) * nowPrevPos.x - Mathf.Sin(90 * Mathf.Deg2Rad) * nowPrevPos.y;
            y = Mathf.Sin(90 * Mathf.Deg2Rad) * nowPrevPos.x + Mathf.Cos(90 * Mathf.Deg2Rad) * nowPrevPos.y;

            Gears[i].point3 = new Vector3(x, y, 0);


            x = Mathf.Cos(-90 * Mathf.Deg2Rad) * nowPrevPos.x - Mathf.Sin(-90 * Mathf.Deg2Rad) * nowPrevPos.y;
            y = Mathf.Sin(-90 * Mathf.Deg2Rad) * nowPrevPos.x + Mathf.Cos(-90 * Mathf.Deg2Rad) * nowPrevPos.y;

            Gears[i].point4 = new Vector3(x, y, 0);



            float angleP1 = change(Mathf.Atan2(Gears[i].point1.x, Gears[i].point1.y) * Mathf.Rad2Deg);
            float angleP2 = change(Mathf.Atan2(Gears[i].point2.x, Gears[i].point2.y) * Mathf.Rad2Deg);
            float angleP3 = change(Mathf.Atan2(Gears[i].point3.x, Gears[i].point3.y) * Mathf.Rad2Deg);
            float angleP4 = change(Mathf.Atan2(Gears[i].point4.x, Gears[i].point4.y) * Mathf.Rad2Deg);



            if (angleP3 <= 180 && angleP3 >= 0)
            {
                if (angleP3 > angleP1  && angleP4 < angleP1)
                {
                    Gears[i].point5 = Gears[i].point1;
                }
                else
                {
                    Gears[i].point5 = Gears[i].point2;
                }
            }else
            {
                if (i == 2)
                {
                    Debug.Log("a");
                }
                if ((angleP3 > angleP1 && -180 <= angleP1) || (angleP4 < angleP1 && 180 >= angleP1))
                {
                    Gears[i].point5 = Gears[i].point1;
                    if (i == 2)
                    {
                        Debug.Log("a");
                    }
                }
                else
                {
                    Gears[i].point5 = Gears[i].point2;
                }
            }


            if (angleP1 <= 180 && angleP1 >= 0)
            {
                if (angleP1 > angleP3 && angleP2 < angleP3)
                {
                    Gears[i].point6 = Gears[i].point3;
                }
                else
                {
                    Gears[i].point6 = Gears[i].point4;
                }
            }
            else
            {

                if ((angleP1 > angleP3 && -180 <= angleP3) || (angleP2 < angleP3 && 180 >= angleP3))
                {
                    Gears[i].point6 = Gears[i].point3;
                }
                else
                {
                    Gears[i].point6 = Gears[i].point4;
                }
            }
            

        }
        BeltPointSet();
    }



    void BeltPointSet()
    {
        int j = 0;
        Debug.Log(Gears.Length);

        for (int i = 0; i < Gears.Length; i++)
        {
            int iNext = i + 1;
            int iPrev = i - 1;

            if (iNext == Gears.Length)
            {
                iNext = 0;
            }

            if (i == 0)
            {
                iPrev = Gears.Length - 1;
            }

            Vector3 pos;

            float a = Mathf.Sqrt(Mathf.Pow(Gears[i].point5.x, 2) + Mathf.Pow(Gears[i].point5.y, 2));
            float b = Mathf.Sqrt(Mathf.Pow(Gears[i].point6.x, 2) + Mathf.Pow(Gears[i].point6.y, 2));
            float angle = Mathf.Acos(((Gears[i].point5.x * Gears[i].point6.x + Gears[i].point5.y * Gears[i].point6.y) / (a * b))) * Mathf.Rad2Deg;




            //点の間隔
            float ax = 0.3f * 360 / ((Gears[i].radius * 2) * 3.14f);

          

            float nowAngle = 0;

            pos = Gears[i].point6;
            this.line.positionCount++;
            line.SetPosition(j, new Vector3(Gears[i].pos.x + Gears[i].point6.x, Gears[i].pos.y + Gears[i].point6.y, 0));

            beltPoints[j].pos = new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0);
            beltPoints[j].moveCountDown = angle - nowAngle;
            beltPoints[j].gearNumber = i;
            beltPoints[j].lien = false;

            j++;

            
            while (angle > nowAngle)
            {
                if ( ((Gears[0].pos.y > Gears[1].pos.y) && (Gears[Gears.Length - 1].pos.y > Gears[1].pos.y)))
                {
                    x = Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.y;
                    y = Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.y;
                }else
                {
                    x = Mathf.Cos(ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(ax * Mathf.Deg2Rad) * pos.y;
                    y = Mathf.Sin(ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(ax * Mathf.Deg2Rad) * pos.y;
                }


                beltPoints[j].pos = new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0);
                beltPoints[j].moveCountDown = angle - nowAngle;
                beltPoints[j].gearNumber = i;
                beltPoints[j].lien = false;

                this.line.positionCount++;
                line.SetPosition(j, new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0));
                j++;

                pos = new Vector3(x, y, 0);

                nowAngle += ax;
            }





            this.line.positionCount++;
                line.SetPosition(j, new Vector3(Gears[i].pos.x + Gears[i].point5.x, Gears[i].pos.y + Gears[i].point5.y, 0));
            
            beltPoints[j].pos = new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0);
            beltPoints[j].moveCountDown = Mathf.Abs(Mathf.Sqrt(Mathf.Pow((Gears[iPrev].pos.x + Gears[iPrev].point6.x) - (Gears[i].pos.x + Gears[i].point5.x), 2) + Mathf.Pow((Gears[iPrev].pos.y + Gears[iPrev].point6.y) - (Gears[i].pos.y + Gears[i].point5.y), 2)));
            beltPoints[j].gearNumber = i;
            beltPoints[j].lien = true;         
            j++;




            Vector3 startPos = new Vector3(Gears[i].pos.x + Gears[i].point5.x, Gears[i].pos.y + Gears[i].point5.y, 0);
            Vector3 endPos = new Vector3(Gears[iNext].pos.x + Gears[iNext].point6.x, Gears[iNext].pos.y + Gears[iNext].point6.y, 0);

            float max = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(endPos.x - startPos.x, 2) + Mathf.Pow(endPos.y - startPos.y, 2)));

            for (float move = 0.3f; move < max; move += 0.3f)
            {
                this.line.positionCount++;
                line.SetPosition(j, Vector3.Lerp(startPos, endPos,  move / max));

                beltPoints[j].pos = Vector3.Lerp(startPos, endPos,  move / max);
                beltPoints[j].moveCountDown = move;
                beltPoints[j].moveCountDownMax = max;
                beltPoints[j].gearNumber = i;
                beltPoints[j].lien = true;

                j++;
            }

        }



        for (int b = 0; b < line.positionCount; b++)
        {
            int nowBeltPointNumber = b;
            int nextBeltPoitNumber = b + 1;

            if (nextBeltPoitNumber == line.positionCount)
            {
                nextBeltPoitNumber = 0;
            }

            float rot = Mathf.Atan2(line.GetPosition(nextBeltPoitNumber).x - line.GetPosition(nowBeltPointNumber).x, line.GetPosition(nextBeltPoitNumber).y - line.GetPosition(nowBeltPointNumber).y) * Mathf.Rad2Deg;

            Vector3 rotation = new Vector3(0, 0, -rot + 90f);

            beltPoints[nowBeltPointNumber].game = Instantiate(p, line.GetPosition(nowBeltPointNumber), Quaternion.Euler(rotation));
        }


        beltState = BeltState.SPIN;
    }



    float change(float a)
    {
        if(Mathf.Abs( a) == 180)
        {
            a = -180;
        }
        return a;
    }

    void Spin()
    {
        for (int i = 1; i < line.positionCount; i++)
        {
            int nowBeltPointNumber = i;
            int nextBeltPoitNumber = i + 1;

            if (nextBeltPoitNumber == line.positionCount)
            {
                nextBeltPoitNumber = 0;
            }


            int gearNumber = beltPoints[i].gearNumber;
            int nextgearNumber = beltPoints[i].gearNumber + 1;

            if (nextgearNumber == Gears.Length)
            {
                nextgearNumber = 0;
            }

            GameObject nowgameObje = beltPoints[nowBeltPointNumber].game;
            GameObject nextgameObje = beltPoints[nextBeltPoitNumber].game;

            if (!beltPoints[i].lien)
            {



                float ax = (0.3f * Time.deltaTime) * 360 / ((Gears[beltPoints[i].gearNumber].radius * 2) * 3.14f);

                beltPoints[i].game.transform.position = new Vector3(nowgameObje.transform.position.x - Gears[gearNumber].pos.x, nowgameObje.transform.position.y - Gears[gearNumber].pos.y);

                x = Mathf.Cos(-ax * Mathf.Deg2Rad) * nowgameObje.transform.position.x - Mathf.Sin(-ax * Mathf.Deg2Rad) * nowgameObje.transform.position.y;
                y = Mathf.Sin(-ax * Mathf.Deg2Rad) * nowgameObje.transform.position.x + Mathf.Cos(-ax * Mathf.Deg2Rad) * nowgameObje.transform.position.y;


                beltPoints[i].moveCountDown -= ax;
                if (beltPoints[i].moveCountDown < 0)
                {
                    beltPoints[i].lien = true;

                    Vector3 startPos = new Vector3(Gears[gearNumber].point5.x + Gears[gearNumber].pos.x, Gears[gearNumber].point5.y + Gears[gearNumber].pos.y);
                    Vector3 endPos = new Vector3(Gears[nextgearNumber].point6.x + Gears[nextgearNumber].pos.x, Gears[nextgearNumber].point6.y + Gears[nextgearNumber].pos.y);


                    beltPoints[i].moveCountDown = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(endPos.x - startPos.x, 2) + Mathf.Pow(endPos.y - startPos.y, 2)));

                    beltPoints[i].moveCountDownMax = beltPoints[i].moveCountDown;


                }


                beltPoints[i].game.transform.position = new Vector3(x + Gears[gearNumber].pos.x, y + Gears[gearNumber].pos.y, 0);


            }
            else
            {


                //float yy = Mathf.Sin(Mathf.Atan2(Gears[nextgearNumber].point6.x - Gears[gearNumber].point5.x, Gears[nextgearNumber].point6.y - Gears[gearNumber].point5.y)) * (0.3f * Time.deltaTime);
                //float xx = Mathf.Cos(Mathf.Atan2(Gears[nextgearNumber].point6.x - Gears[gearNumber].point5.x, Gears[nextgearNumber].point6.y - Gears[gearNumber].point5.y)) * (0.3f * Time.deltaTime);


                Vector3 startPos = new Vector3(Gears[gearNumber].point5.x + Gears[gearNumber].pos.x, Gears[gearNumber].point5.y + Gears[gearNumber].pos.y);
                Vector3 endPos = new Vector3(Gears[nextgearNumber].point6.x + Gears[nextgearNumber].pos.x, Gears[nextgearNumber].point6.y + Gears[nextgearNumber].pos.y);


                beltPoints[i].game.transform.position = Vector3.Lerp(startPos, endPos, 1f - beltPoints[i].moveCountDown / beltPoints[i].moveCountDownMax);


                beltPoints[i].moveCountDown -= (0.3f * Time.deltaTime);


                if (beltPoints[i].moveCountDown < 0)
                {

                    beltPoints[i].lien = false;

                    beltPoints[i].gearNumber++;

                    if (beltPoints[i].gearNumber == Gears.Length)
                    {
                        beltPoints[i].gearNumber = 0;
                    }


                    float a = Mathf.Sqrt(Mathf.Pow(Gears[beltPoints[i].gearNumber].point5.x, 2) + Mathf.Pow(Gears[beltPoints[i].gearNumber].point5.y, 2));
                    float b = Mathf.Sqrt(Mathf.Pow(Gears[beltPoints[i].gearNumber].point6.x, 2) + Mathf.Pow(Gears[beltPoints[i].gearNumber].point6.y, 2));
                    float angle = Mathf.Acos(((Gears[beltPoints[i].gearNumber].point5.x * Gears[beltPoints[i].gearNumber].point6.x + Gears[beltPoints[i].gearNumber].point5.y * Gears[beltPoints[i].gearNumber].point6.y) / (a * b))) * Mathf.Rad2Deg;

                    beltPoints[i].moveCountDown = angle;

                }
            }



            float rot = Mathf.Atan2(nextgameObje.transform.position.x - nowgameObje.transform.position.x, nextgameObje.transform.position.y - nowgameObje.transform.position.y) * Mathf.Rad2Deg;

            Vector3 rotation = new Vector3(0, 0, -rot - 90f);


            beltPoints[i].game.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
