using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Belt : MonoBehaviour
{

    LineRenderer line;
    List<GameObject> list = new List<GameObject>();

    [SerializeField] Camera camera;

    [SerializeField] Texture2D texture;

    public bool belt = false;

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

    int beltPower = 0;

    bool right = false;


    Vector3[] poss = new Vector3[100];


    GameObject[] game = new GameObject[100];


    [SerializeField] GameObject beltPrefab;


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
        public int power;
        public int beltPower;

        public GameObject gear;
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


        public Vector3 startPos;
        public Vector3 endPos;

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
            switch (beltState)
        {
            case BeltState.NOLMAL:
                GearSet();
                break;

            case BeltState.PREPARATION:
                GearStatus2();
                PowerConnect();
                break;

            case BeltState.SPIN:
                Spin();
                break;
        }
       
    }

    void GearSet()
    {
        if (Input.GetMouseButtonUp(0) && belt)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit2d.collider != null && (beltState == BeltState.NOLMAL && hit2d.collider.gameObject.tag == Common.Gear || hit2d.collider.gameObject.tag == Common.StageGear))
            {
                if (Gears[gearsListCount].gear != hit2d.collider.gameObject)
                {
                    Gears[gearsListCount].pos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                                   camera.ScreenToWorldPoint(Input.mousePosition).y,
                                                   0);
                    Gears[gearsListCount].radius = hit2d.collider.gameObject.GetComponent<GearState>().radius / 2f;

                    Gears[gearsListCount].gear = hit2d.collider.gameObject;

                    gearsListCount++;

                    if (gearsListCount == 2)
                    {
                        beltState = BeltState.PREPARATION;
                    }
                }
            }

        }
    }


    void GearStatus2()
    {
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


            Gears[i].point5 = Gears[i].point1;
            Gears[i].point6 = Gears[i].point2;

        }
        BeltPointSet();
    }

    /*
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
                if ((angleP3 > angleP1 && -180 <= angleP1) || (angleP4 < angleP1 && 180 >= angleP1))
                {
                    Gears[i].point5 = Gears[i].point1;
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

            */

    void BeltPointSet()
    {
        int j = 0;
        //Debug.Log(Gears.Length);

        for (int i = 0; i < Gears.Length; i++)
        {

            int iNext = i + 1;
            //int iPrev = i - 1;

            if (iNext == Gears.Length)
            {
                iNext = 0;
            }

            if (i == 0)
            {
                //iPrev = Gears.Length - 1;
            }

            Vector3 pos;

            float a = Mathf.Sqrt(Mathf.Pow(Gears[i].point5.x, 2) + Mathf.Pow(Gears[i].point5.y, 2));
            float b = Mathf.Sqrt(Mathf.Pow(Gears[i].point6.x, 2) + Mathf.Pow(Gears[i].point6.y, 2));
            //float angle = Mathf.Acos(((Gears[i].point5.x * Gears[i].point6.x + Gears[i].point5.y * Gears[i].point6.y) / (a * b))) * Mathf.Rad2Deg;
            float angle = Mathf.Acos(((Gears[beltPoints[i].gearNumber].point5.x * Gears[beltPoints[i].gearNumber].point6.x + Gears[beltPoints[i].gearNumber].point5.y * Gears[beltPoints[i].gearNumber].point6.y) / (a * b))) * Mathf.Rad2Deg;


            //点の間隔
            float ax = 0.3f * 360 / ((Gears[i].radius * 2) * 3.14f);

 
            float nowAngle = 0;

            pos = Gears[i].point6;

            beltPoints[j].pos = new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0);
            beltPoints[j].moveCountDown = angle - nowAngle;
            beltPoints[j].gearNumber = i;
            beltPoints[j].lien = false;

            this.line.positionCount++;
            line.SetPosition(j, new Vector3(Gears[i].pos.x + Gears[i].point6.x, Gears[i].pos.y + Gears[i].point6.y, 0));

            j++;
            while (angle > nowAngle)
            {
                x = Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.y;
                y = Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.y;


                beltPoints[j].pos = new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0);
                if (!right)
                {
                    beltPoints[j].moveCountDown = angle - nowAngle;
                }
                else
                {
                    beltPoints[j].moveCountDown = nowAngle;
                }

                beltPoints[j].gearNumber = i;
                beltPoints[j].lien = false;

                this.line.positionCount++;
                line.SetPosition(j, new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0));
                j++;

                pos = new Vector3(x, y, 0);

                nowAngle += ax;
            }

            /*
            while (angle > nowAngle)
            {

                if (((Gears[2].pos.y < Gears[1].pos.y) && (Gears[2].pos.y < Gears[0].pos.y)) && (((Gears[0].pos.x < Gears[1].pos.x) && (Gears[0].pos.x < Gears[2].pos.x)) || ((Gears[0].pos.x > Gears[1].pos.x) && (Gears[0].pos.x > Gears[2].pos.x))))
                {

                    if (((Gears[0].pos.y > Gears[1].pos.y) && (Gears[0].pos.y > Gears[2].pos.y)) || ((Gears[0].pos.x < Gears[2].pos.x) && (Gears[0].pos.x < Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.y;
                        right = false;
                    }

                    if (((Gears[0].pos.y < Gears[1].pos.y) && (Gears[0].pos.y < Gears[2].pos.y)) || ((Gears[0].pos.x > Gears[2].pos.x) && (Gears[0].pos.x > Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(ax * Mathf.Deg2Rad) * pos.y;
                        right = true;
                    }


                }
                else if(((Gears[2].pos.y > Gears[1].pos.y) && (Gears[2].pos.y > Gears[0].pos.y)) && (((Gears[0].pos.x < Gears[1].pos.x) && (Gears[0].pos.x < Gears[2].pos.x)) || ((Gears[0].pos.x > Gears[1].pos.x) && (Gears[0].pos.x > Gears[2].pos.x))))
                {

                    if (((Gears[0].pos.y > Gears[1].pos.y) && (Gears[0].pos.y > Gears[2].pos.y)) || ((Gears[0].pos.x < Gears[2].pos.x) && (Gears[0].pos.x < Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(ax * Mathf.Deg2Rad) * pos.y;
                        right = true;

                    }

                    if (((Gears[0].pos.y < Gears[1].pos.y) && (Gears[0].pos.y < Gears[2].pos.y)) || ((Gears[0].pos.x > Gears[2].pos.x) && (Gears[0].pos.x > Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.y;
                        right = false;
                    }
 
                }
                else if (((Gears[2].pos.x> Gears[1].pos.x) && (Gears[2].pos.x > Gears[0].pos.x)) && (((Gears[0].pos.y < Gears[1].pos.y) && (Gears[0].pos.x < Gears[2].pos.x)) || ((Gears[0].pos.y > Gears[1].pos.y) && (Gears[0].pos.x < Gears[2].pos.x))))
                {

                    if (((Gears[0].pos.y > Gears[1].pos.y) && (Gears[0].pos.y > Gears[2].pos.y)) || ((Gears[0].pos.x < Gears[2].pos.x) && (Gears[0].pos.x < Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(ax * Mathf.Deg2Rad) * pos.y;
                        right = true;

                    }

                    if (((Gears[0].pos.y < Gears[1].pos.y) && (Gears[0].pos.y < Gears[2].pos.y)) || ((Gears[0].pos.x > Gears[2].pos.x) && (Gears[0].pos.x > Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.y;
                        right = false;
                    }

                }
                else if (((Gears[2].pos.x < Gears[1].pos.x) && (Gears[2].pos.x < Gears[0].pos.x)) && (((Gears[0].pos.y > Gears[1].pos.y) && (Gears[0].pos.x > Gears[2].pos.x)) || ((Gears[0].pos.y < Gears[1].pos.y) && (Gears[0].pos.x > Gears[2].pos.x))))
                {

                    if (((Gears[0].pos.y > Gears[1].pos.y) && (Gears[0].pos.y > Gears[2].pos.y)) || ((Gears[0].pos.x < Gears[2].pos.x) && (Gears[0].pos.x < Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(ax * Mathf.Deg2Rad) * pos.y;
                        right = true;

                    }

                    if (((Gears[0].pos.y < Gears[1].pos.y) && (Gears[0].pos.y < Gears[2].pos.y)) || ((Gears[0].pos.x > Gears[2].pos.x) && (Gears[0].pos.x > Gears[2].pos.x)))
                    {
                        x = Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.x - Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.y;
                        y = Mathf.Sin(-ax * Mathf.Deg2Rad) * pos.x + Mathf.Cos(-ax * Mathf.Deg2Rad) * pos.y;
                        right = false;
                    }
    
                }




            beltPoints[j].pos = new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0);
                if (!right)
                {
                    beltPoints[j].moveCountDown = angle - nowAngle;
                }
                else
                {
                    beltPoints[j].moveCountDown = nowAngle;
                }

                beltPoints[j].gearNumber = i;
                beltPoints[j].lien = false;

                this.line.positionCount++;
                line.SetPosition(j, new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0));
                j++;

                pos = new Vector3(x, y, 0);

                nowAngle += ax;
            }
                            */


            Vector3 startPos = new Vector3(Gears[i].pos.x + Gears[i].point5.x, Gears[i].pos.y + Gears[i].point5.y, 0);
            Vector3 endPos = new Vector3(Gears[iNext].pos.x + Gears[iNext].point6.x, Gears[iNext].pos.y + Gears[iNext].point6.y, 0);

            float max = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(endPos.x - startPos.x, 2) + Mathf.Pow(endPos.y - startPos.y, 2)));



            this.line.positionCount++;
                line.SetPosition(j, new Vector3(Gears[i].pos.x + Gears[i].point5.x, Gears[i].pos.y + Gears[i].point5.y, 0));
            
            beltPoints[j].pos = new Vector3(x + Gears[i].pos.x, y + Gears[i].pos.y, 0);
            beltPoints[j].moveCountDown = Mathf.Abs(Mathf.Sqrt(Mathf.Pow((Gears[iNext].pos.x + Gears[iNext].point6.x) - (Gears[i].pos.x + Gears[i].point5.x), 2) + Mathf.Pow((Gears[iNext].pos.y + Gears[iNext].point6.y) - (Gears[i].pos.y + Gears[i].point5.y), 2)));
            beltPoints[j].moveCountDownMax = max;
            beltPoints[j].gearNumber = i;
            beltPoints[j].lien = true;

           // beltPoints[j].startPos = new Vector3(Gears[i].point5.x + Gears[i].pos.x, Gears[i].point5.y + Gears[i].pos.y);
            //beltPoints[j].endPos = new Vector3(Gears[i + 1].point6.x + Gears[i + 1].pos.x, Gears[i + 1].point6.y + Gears[i + 1].pos.y);
            j++;




            for (float move = 0.3f; move < max; move += 0.3f)
            {
                this.line.positionCount++;
                line.SetPosition(j, Vector3.Lerp(startPos, endPos,  move / max));

                beltPoints[j].pos = Vector3.Lerp(startPos, endPos,  move / max);
                beltPoints[j].moveCountDown = move;
                beltPoints[j].moveCountDownMax = max;
                beltPoints[j].gearNumber = i;
                beltPoints[j].lien = true;


               // beltPoints[j].startPos = new Vector3(Gears[i].point5.x + Gears[i].pos.x, Gears[i].point5.y + Gears[i].pos.y);
               // beltPoints[j].endPos = new Vector3(Gears[i + 1].point6.x + Gears[i + 1].pos.x, Gears[i + 1].point6.y + Gears[i + 1].pos.y);

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

            beltPoints[nowBeltPointNumber].game = Instantiate(beltPrefab, line.GetPosition(nowBeltPointNumber), Quaternion.Euler(rotation));
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

        for (int i = 0; i < line.positionCount; i++)
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

                    if (right)
                    {
                        beltPoints[i].gearNumber--;

                        if (beltPoints[i].gearNumber == -1)
                        {
                            beltPoints[i].gearNumber = Gears.Length - 1;
                        }
                    }
                }


                beltPoints[i].game.transform.position = new Vector3(x + Gears[gearNumber].pos.x, y + Gears[gearNumber].pos.y, 0);




                float rot = Mathf.Atan2(nextgameObje.transform.position.x - nowgameObje.transform.position.x, nextgameObje.transform.position.y - nowgameObje.transform.position.y) * Mathf.Rad2Deg;

                Vector3 rotation = new Vector3(0, 0, -rot + 90f);


                beltPoints[i].game.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                Vector3 startPos = new Vector3(Gears[gearNumber].point5.x + Gears[gearNumber].pos.x, Gears[gearNumber].point5.y + Gears[gearNumber].pos.y);
                Vector3 endPos = new Vector3(Gears[nextgearNumber].point6.x + Gears[nextgearNumber].pos.x, Gears[nextgearNumber].point6.y + Gears[nextgearNumber].pos.y);

                if (right)
                {
                    beltPoints[i].game.transform.position = Vector3.Lerp(startPos, endPos, beltPoints[i].moveCountDown / beltPoints[i].moveCountDownMax);
                }
                else
                {
                    beltPoints[i].game.transform.position = Vector3.Lerp(startPos, endPos, 1f - beltPoints[i].moveCountDown / beltPoints[i].moveCountDownMax);
                }

                beltPoints[i].moveCountDown -= (0.3f * Time.deltaTime);


                if (beltPoints[i].moveCountDown < 0)
                {

                    beltPoints[i].lien = false;

                    if (!right)
                    {
                        beltPoints[i].gearNumber++;
                        if (i == 1)
                        {
                            Debug.Log(beltPoints[i].gearNumber);
                        }
                        if (beltPoints[i].gearNumber == Gears.Length)
                        {
                            beltPoints[i].gearNumber = 0;
                        }
                    }


                    float a = Mathf.Sqrt(Mathf.Pow(Gears[beltPoints[i].gearNumber].point5.x, 2) + Mathf.Pow(Gears[beltPoints[i].gearNumber].point5.y, 2));
                    float b = Mathf.Sqrt(Mathf.Pow(Gears[beltPoints[i].gearNumber].point6.x, 2) + Mathf.Pow(Gears[beltPoints[i].gearNumber].point6.y, 2));
                    float angle = Mathf.Acos(((Gears[beltPoints[i].gearNumber].point5.x * Gears[beltPoints[i].gearNumber].point6.x + Gears[beltPoints[i].gearNumber].point5.y * Gears[beltPoints[i].gearNumber].point6.y) / (a * b))) * Mathf.Rad2Deg;

                    beltPoints[i].moveCountDown = angle;

                }



                float rot = Mathf.Atan2(endPos.x - startPos.x, endPos.y - startPos.y) * Mathf.Rad2Deg;

                Vector3 rotation = new Vector3(0, 0, -rot + 90f);


                beltPoints[i].game.transform.rotation = Quaternion.Euler(rotation);
            }

        }
    }


    void PowerConnect()
    {
        for (int i = 0; i < Gears.Length; i++)
        {
            beltPower += Gears[i].gear.GetComponent<GearState>().gearPower;
        }

        for(int i = 0; i < Gears.Length; i++)
        {
            Gears[i].gear.GetComponent<GearState>().beltPower = beltPower;
        }
    }

    public void CursorChange()
    {

        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);

    }

    public void CursorChangeNormal()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }

    public void BeltFalse()
    {

        for (int i = 0; i < Gears.Length; i++)
        {
            if (Gears[i].pos != Vector3.zero)
            {
                Gears[i].gear.GetComponent<GearState>().beltPower = 0;
            }
        }

        for (int i = 0; i < beltPoints.Length; i++)
        {
            Destroy(beltPoints[i].game);
        }

        beltState = BeltState.NOLMAL;
         gearsListCount = 0;

    }
}
