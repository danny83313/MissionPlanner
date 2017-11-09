using System;
using System.Linq;
using System.Collections.Generic;
using MissionPlanner.Utilities;

namespace PathProgram
{
    public class PathProgramming
    {

        static public List<PointLatLngAlt> dll_goallist = new List<PointLatLngAlt>();
        static public List<PointLatLngAlt> dll_noflylist = new List<PointLatLngAlt>();
        static public List<PointLatLngAlt> dll_exnoflylist = new List<PointLatLngAlt>();

        static int latLength = dll_goallist.Count;
        static int nflatLength = dll_noflylist.Count;
        static double[,] Cij = new double[latLength, latLength];//大距離陣列*****
        //static int[] Cij1111 = new int[latLength]; 9/30
        static int m_num = 1;//群組數****
        static int[,] multi_path = new int[m_num, latLength - m_num + 3];//多路徑目標點順序[群組數,目標點-群組數+X] 9/30
        static int[] point_num_of_multipath = new int[m_num];//各群目標點數量***
        static int[] astar_ans = new int[nflatLength];
        static int[,] save_turning_point = new int[latLength * 2 + nflatLength, nflatLength + 2];
        static int[,] opt_multi_path = new int[m_num, latLength - m_num + 3];
        static int[,] path_solution = new int[m_num, ((latLength - m_num + 3) - 1) * 2 + nflatLength];
        static int iterat = 0;//迭代
        static int tabulist_length = 10;//禁忌名單長度
        /*public void math(double inlat, double inlng, int inalt, out double lat, out double lng, out int alt)
        {
           
                lat = 0.0005 + inlat;
                lng = 0.0005 + inlng;
                alt = 10;
            
        }*/
        /*public void math(double homelat ,double homelng,double[] inlat, double[] inlng, int[] inalt, out double[] lat, out double[] lng, out int[] alt)
        {
            lat = new double[inlat.Length];
            lng = new double[inlat.Length];
            alt = new int[inlat.Length];
            for (int i = 0; i < inlat.Length; i++)
            {
                lat[i] = 0.0005 + inlat[i];
                lng[i] = 0.0005 + inlng[i];
                alt[i] = 10;
            }
           
        }*/

        public void math(int groupset, List<PointLatLngAlt> Allpointlist, List<PointLatLngAlt> noflypointlist,
                         ref List<PointLatLngAlt> Apointlist, ref List<PointLatLngAlt> Bpointlist, ref List<PointLatLngAlt> Cpointlist)
        {
            List<PointLatLngAlt> final_list = new List<PointLatLngAlt>();
            Apointlist.Clear();
            dll_goallist.Clear();
            final_list.Clear();
            dll_noflylist.Clear();
            dll_exnoflylist.Clear();
            /////////////////
            for (int i = 0; i < Allpointlist.Count; i++)
            {//存目標點經緯
                dll_goallist.Add(new PointLatLngAlt(Allpointlist[i].Lat, Allpointlist[i].Lng, Allpointlist[i].Alt));
            }

            for (int i = 0; i < noflypointlist.Count; i++)
            {//存禁航區經緯
                dll_noflylist.Add(new PointLatLngAlt(noflypointlist[i].Lat, noflypointlist[i].Lng, noflypointlist[i].Alt));
            }
            if (dll_noflylist.Count != 0) dll_noflylist.Add(new PointLatLngAlt(noflypointlist[0].Lat, noflypointlist[0].Lng, noflypointlist[0].Alt));//最後補禁航區第一點*/

            ///////////宣告
            latLength = dll_goallist.Count;
            if (dll_noflylist.Count == 0) nflatLength = dll_noflylist.Count;//無禁航區
            else
            {//有禁航區
                nflatLength = dll_noflylist.Count - 1;
                exnofly();//轉擇點
            }
            Cij = new double[latLength, latLength];
            multi_path = new int[m_num, latLength - m_num + 3];
            point_num_of_multipath = new int[m_num];
            save_turning_point = new int[latLength * latLength, nflatLength + 2];
            opt_multi_path = new int[m_num, latLength - m_num + 3];
            path_solution = new int[m_num, ((latLength - m_num + 3) - 1) * 2 + nflatLength];
            astar_ans = new int[nflatLength];

            ///////////////


            distance();//大距離矩陣

            Array.Copy(improve(), opt_multi_path, opt_multi_path.Length);
            for (int i = 0; i < m_num; i++)
            {
                for (int j = 0; j < latLength - m_num + 3; j++)
                {//存目標點
                    path_solution[i, j] = opt_multi_path[i, j];
                }
            }



            for (int i = 0; i < m_num; i++)
            {
                for (int j = 0; j < latLength - m_num + 1; j++)
                {//演算後 點轉經緯
                    final_list.Add(new PointLatLngAlt(dll_goallist[path_solution[i, j]].Lat, dll_goallist[path_solution[i, j]].Lng, dll_goallist[path_solution[i, j]].Alt));
                }
            }

            if (dll_noflylist.Count != 0) insert();//如果有禁航區->插入禁航區*/


            /*for (int i = 0; i < Allpointlist.Count-1; i++)
            {
              //  Outpointlist.Add(new PointLatLngAlt(Allpointlist[].Lat, Allpointlist[].Lng , Allpointlist[].Alt));
                Outpointlist.Add(new PointLatLngAlt(Allpointlist[i + 1].Lat + 0.005, Allpointlist[i + 1].Lng + 0.005, 10));
               
            }*/
            Apointlist.Add(new PointLatLngAlt(Allpointlist[0].Lat, Allpointlist[0].Lng, Allpointlist[0].Alt));
            for (int i = 0; i < Allpointlist.Count - 1; i++)
            {
                Apointlist.Add(new PointLatLngAlt(final_list[i + 1].Lat, final_list[i + 1].Lng, final_list[i + 1].Alt));
            }
            Apointlist.Add(new PointLatLngAlt(Allpointlist[0].Lat, Allpointlist[0].Lng, Allpointlist[0].Alt));
        }


        static private int[,] improve()//改良後路徑(產生最終解) ******
        {

            multi_path = initail();//單路徑TABU->分割完成


            int[,] opt_path = new int[m_num, latLength - m_num + 3];
            Array.Copy(multi_path, opt_path, multi_path.Length);
            double opt_max = 0;//最佳
            double opt_total = 0;
            for (int i = 0; i < m_num; i++)
            {
                int[] path_crs_tabu = new int[latLength - m_num + 3];
                for (int j = 0; j < latLength - m_num + 3; j++)//取出
                {
                    path_crs_tabu[j] = opt_path[i, j];
                }
                double dist_f = path_distance(path_crs_tabu);//計算交換過程的單群距離
                opt_total += dist_f;//持續累加總距離
                if (dist_f > opt_max) opt_max = dist_f;   //新找到>最大距離 新取代舊
            }
            int count = 0;

            do
            {
                Array.Copy(Tabu_cross_route(multi_path), multi_path, multi_path.Length);//進入多路徑TABU

                for (int i = 0; i < m_num; i++)//in_route 個別對每一群的單一路徑做改善
                {
                    int[] single_path_of_multi = new int[point_num_of_multipath[i] + 2];//每個路徑長度
                    for (int j = 0; j < point_num_of_multipath[i] + 2; j++)//取出路徑
                    {
                        single_path_of_multi[j] = multi_path[i, j];
                    }

                    Array.Copy(Tabu_in_route(single_path_of_multi), single_path_of_multi, single_path_of_multi.Length);//第二次 單路徑TABU
                    //single_path_of_multi = Tabu_in_route(single_path_of_multi);

                    for (int j = 0; j < point_num_of_multipath[i] + 2; j++)
                    {
                        multi_path[i, j] = single_path_of_multi[j];
                    }
                }//

                double cur_max = 0;//當前單群距離最大
                double cur_total = 0;//當前總距離最大
                for (int i = 0; i < m_num; i++)
                {
                    int[] path_f = new int[latLength - m_num + 3];
                    for (int j = 0; j < latLength - m_num + 3; j++)//取出
                    {
                        path_f[j] = multi_path[i, j];
                    }
                    double dist_f = path_distance(path_f);//計算交換過程的單群距離
                    cur_total += dist_f;//持續累加總距離
                    if (dist_f > cur_max) cur_max = dist_f;   //新找到>最大距離 新取代舊
                }


                if (compare_max_total(cur_max, cur_total, opt_max, opt_total))
                {
                    opt_max = cur_max;
                    opt_total = cur_total;
                    Array.Copy(multi_path, opt_path, multi_path.Length);
                    count = 0;
                }
                else
                {
                    count++;
                }
                iterat++;//迭代++
            } while (!(count == 2));


            return opt_path;
        }

        static private int[,] initail()//分割後 初始路徑
        {
            int[] single_path = new int[latLength + 1];// 單群路徑(長度+原點)
            Array.Copy(Tabu_in_route(NDM()), single_path, latLength + 1);//將最近距離法回傳之路徑 代入單路徑TABU
            return cut(single_path);
        }

        static private int[,] cut(int[] single_path)//路線分割///////b
        {
            int[] cut_path = new int[0];
            double cut_path_d = 0;
            double cut_index_d = path_distance(single_path) / m_num; //總路徑距離除以機器數(均分距離)
            //int[,] multi_path = new int[m_num, latLength-1]; 9/30
            for (int i = 0; i < m_num - 1; i++)//主要分割
            {
                int j = 1;
                do
                {
                    j++;
                    cut_path = new int[j];
                    Array.Copy(single_path, cut_path, j);
                    cut_path_d = path_distance(cut_path);//切出的路徑距離 不含回原點
                } while (!((cut_path_d > cut_index_d))); //停止條件 切完距離>平均距離

                if (cut_path.Length == 2) //第一點距離超過 至少拿一點////
                {
                    for (int k = 0; k < cut_path.Length; k++)//10/19
                    {
                        multi_path[i, k] = cut_path[k];
                    }
                    for (int k = 1; k < single_path.Length - cut_path.Length + 1; k++)
                    {//將剩下的目標點前移
                        single_path[k] = single_path[cut_path.Length - 1 + k];
                    }
                    point_num_of_multipath[i] = j - 1;//第1~(m_num-1)群 目標點數量
                }
                else //正常分割
                {
                    for (int k = 0; k < cut_path.Length - 1; k++)//10/2
                    {
                        multi_path[i, k] = cut_path[k];
                    }
                    for (int k = 1; k < single_path.Length - cut_path.Length + 2; k++)
                    {//將剩下的目標點前移
                        single_path[k] = single_path[cut_path.Length - 2 + k];
                    }
                    point_num_of_multipath[i] = j - 2;//第1~(m_num-1)群 目標點數量
                }

                multi_path[i, cut_path.Length] = 0;//最後插入原點(回到原點) 10/2

                Array.Resize(ref single_path, single_path.Length - cut_path.Length + 2);

            }

            for (int i = 0; i < single_path.Length; i++)//10/2
            {
                multi_path[m_num - 1, i] = single_path[i];//各群拜訪順序
            }
            int ii = 1;
            do
            {
                ii++;
            } while (!(multi_path[m_num - 1, ii] == 0));
            point_num_of_multipath[m_num - 1] = ii - 1;//最後一群 目標點數量

            return multi_path;
        }

        static private int insert()//tabu後路徑插入禁航區
        {
            for (int i = 0; i < m_num; i++)
            {
                int[] path = new int[((latLength - m_num + 3) - 1) * nflatLength];
                for (int j = 0; j < path_solution.GetLength(1); j++)//取出
                {
                    path[j] = path_solution[i, j];
                }
                int k = 0;
                int n = 0;
                do
                {
                    n = 0;
                    for (int l = 0; l < latLength * latLength; l++)
                    {//有航經禁航區 掃描列
                        if (path[k + n] == save_turning_point[l, 0] && path[k + n + 1] == save_turning_point[l, 1])//
                        {
                            int f = 2;
                            do
                            {
                                f++;
                            } while (!(save_turning_point[l, f] == 0));
                            int[] path1 = new int[((latLength - m_num + 3) - 1) * nflatLength];
                            Array.Copy(path, path1, path1.Length);
                            for (int m = 0; m < path_solution.GetLength(1) - (k + 1) - (f - 2); m++)
                            {
                                path[k + n + 1 + (f - 2) + m] = path1[k + n + 1 + m];
                            }
                            for (int m = 0; m < f - 2; m++)
                            {
                                path[k + n + 1 + m] = save_turning_point[l, 2 + m];
                            }
                            n += f - 2;
                            break;
                        }

                    }
                    k++;
                    for (int j = 0; j < path_solution.Length; j++)
                    {
                        path_solution[i, j] = path[j];
                    }
                } while (!(path[k + n] == 0));
            }
            return 0;
        }

        static private int[,] Tabu_cross_route(int[,] multi_path)//多路徑TABU//
        {
            int[,] current_solution = new int[multi_path.GetLength(0), multi_path.GetLength(1)];//初始值
            Array.Copy(multi_path, current_solution, multi_path.Length);

            double[,] tabulist = new double[tabulist_length, 2];//mix & total皆加入禁忌名單
            int tabu_cnt = 0;//tabulist迭代
            int[,] global_opt_path = new int[multi_path.GetLength(0), multi_path.GetLength(1)];
            Array.Copy(current_solution, global_opt_path, current_solution.Length);
            double global_opt_max = 0;
            double global_opt_total = 0;
            for (int i = 0; i < m_num; i++)
            {
                int[] path_f = new int[latLength - m_num + 3];//正轉路徑
                for (int j = 0; j < latLength - m_num + 3; j++)//取出
                {
                    path_f[j] = global_opt_path[i, j];
                }
                double dist_f = path_distance(path_f);//計算交換過程的單群距離
                global_opt_total += dist_f;//持續累加總距離
                if (dist_f > global_opt_max) global_opt_max = dist_f;   //新找到>最大距離 新取代舊
            }

            int[,] global_sub_path = new int[multi_path.GetLength(0), multi_path.GetLength(1)];
            Array.Copy(current_solution, global_sub_path, current_solution.Length);
            double global_sub_max = double.MaxValue;//全域最佳(最終解)
            double global_sub_total = double.MaxValue;

            double initail_max = 0;//初始
            double initail_total = 0;
            for (int i = 0; i < m_num; i++)
            {
                int[] path_initail = new int[latLength - m_num + 3];
                for (int j = 0; j < latLength - m_num + 3; j++)//取出
                {
                    path_initail[j] = multi_path[i, j];
                }
                double dist_f = path_distance(path_initail);//計算交換過程的單群距離
                initail_total += dist_f;//持續累加總距離
                if (dist_f > initail_max) initail_max = dist_f;   //新找到>最大距離 新取代舊
            }


            int count = 0;
            do
            {
                int[,] near_opt_path = new int[multi_path.GetLength(0), multi_path.GetLength(1)];
                double near_opt_max = double.MaxValue;
                double near_opt_total = double.MaxValue;

                for (int i = 0; i < m_num - 1; i++)//主群
                {
                    for (int j = 0; j < m_num - 1 - i; j++)//副群
                    {
                        for (int k = 0; k < point_num_of_multipath[i] + 1; k++)//主群 分割第k個斷點
                        {
                            int[] first_cut_head_path = new int[k + 1];//主群前段
                            int[] first_cut_tail_path = new int[point_num_of_multipath[i] + 1 - k];//主群後段
                            for (int l = 0; l < k + 1; l++)//從總路徑(主群) 分割 取前段
                            {
                                first_cut_head_path[l] = current_solution[i, l];
                            }
                            for (int l = 0; l < point_num_of_multipath[i] + 1 - k; l++)//從總路徑(主群) 分割 取後段
                            {
                                first_cut_tail_path[l] = current_solution[i, k + l + 1];
                            }

                            for (int l = 0; l < point_num_of_multipath[i + j + 1] + 1; l++)//副群 分割第L個斷點
                            {
                                int[] second_cut_head_path = new int[l + 1];//副群前段
                                int[] second_cut_tail_path = new int[point_num_of_multipath[i + j + 1] + 1 - l];//副群後段
                                for (int m = 0; m < l + 1; m++)//從總路徑(副群) 分割 取前段
                                {
                                    second_cut_head_path[m] = current_solution[i + j + 1, m];
                                }
                                for (int m = 0; m < point_num_of_multipath[i + j + 1] + 1 - l; m++)//從總路徑(副群) 分割 取後段
                                {
                                    second_cut_tail_path[m] = current_solution[i + j + 1, l + m + 1];
                                }

                                int[,] multi_path_forward = new int[m_num, latLength - m_num + 3];//正轉
                                Array.Copy(current_solution, multi_path_forward, current_solution.Length);
                                int[,] multi_path_reverse = new int[m_num, latLength - m_num + 3];//反轉
                                Array.Copy(current_solution, multi_path_reverse, current_solution.Length);

                                //Array.Copy(來源arr,開始位置,複製arr,開始存放位置,總數目)

                                ///////正轉
                                for (int m = 0; m < k + 1; m++)//1頭接2尾
                                {
                                    multi_path_forward[i, m] = first_cut_head_path[m];
                                }
                                for (int m = 0; m < point_num_of_multipath[i + j + 1] + 1 - l; m++)
                                {
                                    multi_path_forward[i, m + k + 1] = second_cut_tail_path[m];
                                }
                                for (int m = 0; m < multi_path_forward.GetLength(1) - (k + point_num_of_multipath[i + j + 1] + 2 - l); m++)
                                {
                                    multi_path_forward[i, point_num_of_multipath[i + j + 1] - l + k + 2 + m] = 0;//剩餘位歸0
                                }

                                for (int m = 0; m < l + 1; m++)//2頭接1尾
                                {
                                    multi_path_forward[i + j + 1, m] = second_cut_head_path[m];
                                }
                                for (int m = 0; m < point_num_of_multipath[i] + 1 - k; m++)
                                {
                                    multi_path_forward[i + j + 1, m + l + 1] = first_cut_tail_path[m];
                                }
                                for (int m = 0; m < multi_path_forward.GetLength(1) - (l + point_num_of_multipath[i] + 2 - k); m++)
                                {
                                    multi_path_forward[i + j + 1, point_num_of_multipath[i] + 1 - k + l + 1 + m] = 0;
                                }

                                /////////反轉
                                for (int m = 0; m < k + 1; m++)//1頭接(2頭反轉)
                                {
                                    multi_path_reverse[i, m] = first_cut_head_path[m];
                                }
                                for (int m = 0; m < l + 1; m++)
                                {
                                    multi_path_reverse[i, m + k + 1] = second_cut_head_path[l - m];
                                }
                                for (int m = 0; m < multi_path_forward.GetLength(1) - (k + l + 2); m++)
                                {
                                    multi_path_reverse[i, k + 2 + l + m] = 0;
                                }

                                for (int m = 0; m < point_num_of_multipath[i + j + 1] + 1 - l; m++)//(2尾反轉)接1尾
                                {
                                    multi_path_reverse[i + j + 1, m] = second_cut_tail_path[point_num_of_multipath[i + j + 1] - l - m];
                                }
                                for (int m = 0; m < point_num_of_multipath[i] + 1 - k; m++)
                                {
                                    multi_path_reverse[i + j + 1, m + point_num_of_multipath[i + j + 1] + 1 - l] = first_cut_tail_path[m];
                                }
                                for (int m = 0; m < multi_path_forward.GetLength(1) - (point_num_of_multipath[i + j + 1] + 1 - l + (point_num_of_multipath[i] + 1 - k)); m++)
                                {
                                    multi_path_reverse[i + j + 1, (point_num_of_multipath[i + j + 1] + 1 - l + (point_num_of_multipath[i] + 1 - k)) + m] = 0;
                                }

                                double max_dist_f = 0;//最大距離(正轉)
                                double total_dist_f = 0;//總距離(正轉)
                                double max_dist_r = 0;//(反轉)
                                double total_dist_r = 0;
                                bool not_feasible_f = false;//正轉 非可行解
                                bool not_feasible_r = false;
                                for (int m = 0; m < m_num; m++)
                                {
                                    int[] path_f = new int[latLength - m_num + 3];//正轉路徑
                                    int[] path_r = new int[latLength - m_num + 3];//反轉路徑
                                    for (int n = 0; n < latLength - m_num + 3; n++)//取出
                                    {
                                        path_f[n] = multi_path_forward[m, n];
                                        path_r[n] = multi_path_reverse[m, n];

                                    }
                                    double dist_f = path_distance(path_f);//計算交換過程的單群距離
                                    double dist_r = path_distance(path_r);
                                    if (dist_f == 0) not_feasible_f = true;//若距離=0 ,非可行解=TRUE
                                    if (dist_r == 0) not_feasible_r = true;
                                    total_dist_f += dist_f;//持續累加總距離
                                    total_dist_r += dist_r;
                                    if (dist_f > max_dist_f) max_dist_f = dist_f;   //新找到>最大距離 新取代舊
                                    if (dist_r > max_dist_r) max_dist_r = dist_r;   //"針對m_num群中 距離最大的該群優化，以降低總任務時間"
                                }

                                for (int m = 0; m < tabulist_length; m++)
                                {//非可行解判斷(距離非0 | 不在tabulist)
                                    not_feasible_f |= tabulist[m, 0] == max_dist_f && tabulist[m, 1] == total_dist_f;
                                    not_feasible_r |= tabulist[m, 0] == max_dist_r && tabulist[m, 1] == total_dist_r;
                                }


                                if (compare_max_total(max_dist_f, total_dist_f, max_dist_r, total_dist_r))//max total 
                                {//f win
                                    if (not_feasible_f == true)
                                    {//f非可行解
                                        if (not_feasible_r == true)
                                        {//r也非可行解
                                            //都不行 找下一代
                                        }
                                        else
                                        {//r
                                            if (compare_max_total(max_dist_r, total_dist_r, near_opt_max, near_opt_total))
                                            {//r取代局部最佳解
                                                near_opt_max = max_dist_r;
                                                near_opt_total = total_dist_r;
                                                Array.Copy(multi_path_reverse, near_opt_path, multi_path_reverse.Length);
                                            }
                                        }
                                    }
                                    else
                                    {//f win且可行
                                        if (compare_max_total(max_dist_f, total_dist_f, near_opt_max, near_opt_total))
                                        {//f取代局部最佳解
                                            near_opt_max = max_dist_f;
                                            near_opt_total = total_dist_f;
                                            Array.Copy(multi_path_forward, near_opt_path, multi_path_forward.Length);
                                        }
                                    }
                                }
                                else
                                {//r win
                                    if (not_feasible_r == true)
                                    {//r非可行解
                                        if (not_feasible_f == true)
                                        {//f也非可行解
                                            //都不行 找下一代
                                        }
                                        else
                                        {//f
                                            if (compare_max_total(max_dist_f, total_dist_f, near_opt_max, near_opt_total))
                                            {//f取代局部最佳解
                                                near_opt_max = max_dist_f;
                                                near_opt_total = total_dist_f;
                                                Array.Copy(multi_path_forward, near_opt_path, multi_path_forward.Length);
                                            }
                                        }
                                    }
                                    else
                                    {//r win 且可行
                                        if (compare_max_total(max_dist_r, total_dist_r, near_opt_max, near_opt_total))
                                        {//r取代局部最佳解
                                            near_opt_max = max_dist_r;
                                            near_opt_total = total_dist_r;
                                            Array.Copy(multi_path_reverse, near_opt_path, multi_path_reverse.Length);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Array.Copy(near_opt_path, current_solution, near_opt_path.Length);
                for (int i = 0; i < m_num; i++)
                {
                    int j = 0;
                    do
                    {
                        j++;
                    } while (!(current_solution[i, j] == 0));
                    point_num_of_multipath[i] = j - 1;
                }

                tabulist[tabu_cnt, 0] = near_opt_max;//禁忌名單
                tabulist[tabu_cnt, 1] = near_opt_total;
                tabu_cnt++;
                if (tabu_cnt == tabulist_length) tabu_cnt = 0;//記憶10個 先進先覆蓋

                if (compare_max_total(near_opt_max, near_opt_total, global_opt_max, global_opt_total))
                {
                    global_opt_max = near_opt_max;
                    global_opt_total = near_opt_total;
                    Array.Copy(near_opt_path, global_opt_path, near_opt_path.Length);
                    count = 0;
                }
                else
                {
                    count++;
                    if (compare_max_total(near_opt_max, near_opt_total, global_sub_max, global_sub_total) && (near_opt_max != global_opt_max || near_opt_total != global_opt_total))
                    {
                        global_sub_max = near_opt_max;
                        global_sub_total = near_opt_total;
                        Array.Copy(near_opt_path, global_sub_path, near_opt_path.Length);
                    }
                }


            } while (!(count == 50));
            if (iterat >= 1)//迭代>=1
            {
                if ((global_opt_max == initail_max) && (global_opt_total == initail_total))
                {
                    Array.Copy(global_sub_path, global_opt_path, global_sub_path.Length);
                }
            }

            for (int i = 0; i < m_num; i++)
            {
                int j = 0;
                do
                {
                    j++;
                } while (!(global_opt_path[i, j] == 0));
                point_num_of_multipath[i] = j - 1;
            }
            return global_opt_path;
        }

        static private int[] Tabu_in_route(int[] ndm) //單路徑tabu//<<this
        {
            int[] current_solution = new int[ndm.Length]; //當前解<<this
            Array.Copy(ndm, current_solution, ndm.Length);

            double[] tabulist = new double[tabulist_length];
            int L = 0;//迭代數
            int[] sole_opt_path = new int[ndm.Length];//global
            Array.Copy(ndm, sole_opt_path, ndm.Length);
            double opt_path_d = path_distance(sole_opt_path);
            int no_change = 0;

            do
            {
                int[] near_opt_path = new int[ndm.Length]; //鄰近最佳解路徑
                double near_opt_d = double.MaxValue; //鄰近最佳解距離
                for (int i = 0; i < ndm.Length - 3; i++) //依規律排路徑
                {
                    int[] path_1 = new int[ndm.Length];
                    for (int j = 0; j < i + 1; j++)
                    {//path1(順序不便 位數遞增1++)
                        path_1[j] = current_solution[j];
                    }//

                    int Nj;
                    if (i < 2) Nj = ndm.Length - 4;//10/19
                    else Nj = ndm.Length - 1 - (i + 2);

                    for (int j = 0; j < Nj; j++)
                    {
                        for (int k = 0; k < j + 2; k++)
                        {//path2(順序反向 位數遞增2++)
                            path_1[i + 1 + k] = current_solution[i + j + 2 - k];//正放 反拿
                        }

                        for (int k = 0; k < ndm.Length - 1 - (i + j + 2); k++)
                        {//path3(剩下全部不變)
                            path_1[i + j + 3 + k] = current_solution[i + j + k + 3];
                        }

                        double path_1_d = path_distance(path_1);
                        bool in_list = false;
                        for (int k = 0; k < tabulist.Length; k++)
                        {
                            in_list |= (path_1_d == tabulist[k]);
                        }

                        if ((path_1_d < near_opt_d) && !(in_list)) //(當前距離<鄰近最佳距離)&不在禁忌名單中
                        {
                            near_opt_d = path_1_d;
                            Array.Copy(path_1, near_opt_path, path_1.Length);
                            //near_opt_path = path_1;//<<this
                        }
                    }
                }

                tabulist[L] = near_opt_d;//最佳解加入禁忌名單
                L++;
                if (L == tabulist_length) L = 0;//先進先出

                if (near_opt_d < opt_path_d)//
                {
                    opt_path_d = near_opt_d;
                    Array.Copy(near_opt_path, sole_opt_path, ndm.Length);
                    no_change = 0;//若更新 未改善迭代數歸0
                }
                else no_change++;
                Array.Copy(near_opt_path, current_solution, near_opt_path.Length);

            } while (!(no_change == 12));//終止條件 X代未改善

            return sole_opt_path;
        }

        static private double path_distance(int[] path) //初始解距離
        {
            double value;
            int path_length = path.Length - 1;/////
            double path_dist = 0;
            for (int i = 0; i < path_length; i++)
            {
                value = Cij[path[i], path[i + 1]];
                if (value >= double.MaxValue) value = 0;
                path_dist += value;
            }
            if (path_dist == 0) path_dist = double.MaxValue;

            return path_dist;
        }

        static private int[] NDM() //最近距離法//回傳路徑
        {
            int[] NDM = new int[latLength + 1];
            NDM[0] = 0;
            double[,] Cij_copy = new double[Cij.GetLength(0), Cij.GetLength(1)];//<<this
            Array.Copy(Cij, Cij_copy, Cij.GetLength(0) * Cij.GetLength(1)); //複製一個大矩陣
            // Array.Copy(來源陣列,複製後陣列,內容數目)

            /*for (int i = 0; i < Cij.GetLength(0); i++)
            {
                for (int j = 0; j < Cij.GetLength(0); j++)
                {
                    Cij_copy[i, j] = Cij[i, j];
                }
            }*/

            for (int i = 0; i < latLength; i++)
            {
                Cij_copy[0, i] = double.MaxValue; //第0row=無限大
            }
            for (int i = 0; i < latLength - 1; i++)
            {
                double min_value = double.MaxValue;
                int min_row = int.MaxValue;
                for (int j = 0; j < latLength; j++)
                {
                    for (int k = 0; k < latLength; k++)
                    {
                        if (Cij_copy[j, k] < min_value)//除了第0row找最小值
                        {
                            min_value = Cij_copy[j, k];
                            min_row = j;
                        }
                    }
                }
                for (int j = 0; j < latLength; j++)
                {
                    Cij_copy[min_row, j] = double.MaxValue; //最小值的row=無限大
                }
                NDM[i + 1] = min_row; //初始值(一維)
            }

            NDM[latLength] = 0;
            return NDM;
        }

        static private bool compare_max_total(double a_max, double a_total, double b_max, double b_total)/////
        {
            bool ask_tf = false;
            if (a_max < b_max) ask_tf = true;//
            else
            {
                if (a_max == b_max)
                {
                    if (a_total < b_total) ask_tf = true;
                    else ask_tf = false;
                }
                else ask_tf = false;
            }

            return ask_tf;
        }

        static private double[,] distance() //距離陣列/////////////astar
        {
            int count = 0;
            for (int i = 0; i < Cij.GetLength(0); i++)

            {
                for (int j = 0; j < Cij.GetLength(0); j++)
                {
                    bool x = false;

                    for (int k = 0; k < nflatLength; k++)
                    {
                        x = x || crsnofly(dll_goallist[i].Lat, dll_goallist[i].Lng, dll_goallist[j].Lat, dll_goallist[j].Lng,
                                          dll_noflylist[k].Lat, dll_noflylist[k].Lng, dll_noflylist[k + 1].Lat, dll_noflylist[k + 1].Lng);
                    }
                    if (x == false)
                    {
                        Cij[i, j] = dist(dll_goallist[i].Lat, dll_goallist[i].Lng, dll_goallist[j].Lat, dll_goallist[j].Lng);
                    }
                    else
                    {
                        Cij[i, j] = astar(dll_goallist[i].Lat, dll_goallist[i].Lng, dll_goallist[j].Lat, dll_goallist[j].Lng); //A*
                        save_turning_point[count, 0] = i;
                        save_turning_point[count, 1] = j;
                        int f = 1;
                        do
                        {
                            f++;
                        } while (!(astar_ans[f] == 0));

                        for (int k = 0; k < f - 1; k++)
                        {
                            save_turning_point[count, 2 + k] = astar_ans[f - 1 - k] + latLength - 1;
                        }
                        count++;
                    }
                }

            }
            return Cij;
        }

        static private bool crsnofly(double lat0, double lng0, double lat1, double lng1, double lat2, double lng2, double lat3, double lng3)//判斷路線是否交叉nofly
        {
            {
                double d1 = crsproduct(lat2, lng2, lat3, lng3, lat0, lng0);//3,4,1
                double d2 = crsproduct(lat2, lng2, lat3, lng3, lat1, lng1);//3,4,2
                double d3 = crsproduct(lat0, lng0, lat1, lng1, lat2, lng2);//1,2,3
                double d4 = crsproduct(lat0, lng0, lat1, lng1, lat3, lng3);//1,2,4

                if (d1 * d2 < 0 && d3 * d4 < 0)
                {
                    return true; //有交叉
                }
                else if (d1 == 0 && sameline(lat2, lng2, lat3, lng3, lat0, lng0))
                {
                    return true;
                }
                else if (d2 == 0 && sameline(lat2, lng2, lat3, lng3, lat1, lng1))
                {
                    return true;
                }
                else if (d3 == 0 && sameline(lat0, lng0, lat1, lng1, lat2, lng2))
                {
                    return true;
                }
                else if (d4 == 0 && sameline(lat0, lng0, lat1, lng1, lat3, lng3))
                {
                    return true;
                }
                else
                {
                    return false; //沒交叉
                }

            }
        }

        static private double crsproduct(double lat0, double lng0, double lat1, double lng1, double noflylat0, double noflylng0) //叉積 正負號
        {
            return ((noflylat0 - lat0) * (lng1 - lng0)) - ((lat1 - lat0) * (noflylng0 - lng0));
        }

        static private bool sameline(double lat0, double lng0, double lat1, double lng1, double noflylat0, double noflylng0) //共線判斷
        {
            double minx = Math.Min(lat0, lat1);
            double maxx = Math.Max(lat0, lat1);
            double miny = Math.Min(lng0, lng1);
            double maxy = Math.Max(lng0, lng1);

            return noflylat0 >= minx && noflylat0 <= maxx && noflylng0 >= miny && noflylng0 <= maxy;
        }

        static private double dist(double lat0, double lng0, double lat1, double lng1)//距離function
        {

            double X = Math.Cos(lat0 * Math.PI / 180) * Math.Cos(lng0 * Math.PI / 180) -
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lng1 * Math.PI / 180);

            double Y = Math.Cos(lat0 * Math.PI / 180) * Math.Sin(lng0 * Math.PI / 180) -
                       Math.Cos(lat1 * Math.PI / 180) * Math.Sin(lng1 * Math.PI / 180);

            double Z = Math.Sin(lat0 * Math.PI / 180) - Math.Sin(lat1 * Math.PI / 180);

            double Tij = (2 * 6378137 * Math.Sinh(Math.Sqrt(Math.Pow(X, 2) +
                                                            Math.Pow(Y, 2) +
                                                            Math.Pow(Z, 2)) / 2));

            if (Tij == 0)
            {
                return double.MaxValue;
            }
            else
            {
                return Tij;
            }
        }

        static private double exnofly()//擴大禁航區-轉折點
        {//dll_noflylist.Sum(sumlat => sumlat.Lat) 
            double pclat = ((dll_noflylist.Sum(sumlat => sumlat.Lat)) - (dll_noflylist[dll_noflylist.Count - 1].Lat)) / (dll_noflylist.Count - 1);
            double pclng = ((dll_noflylist.Sum(sumlat => sumlat.Lng)) - (dll_noflylist[dll_noflylist.Count - 1].Lng)) / (dll_noflylist.Count - 1);
            for (int i = 0; i < dll_noflylist.Count - 1; i++)
            {
                dll_exnoflylist.Add(new PointLatLngAlt(pclat + 1.25 * (dll_noflylist[i].Lat - pclat), pclng + 1.25 * (dll_noflylist[i].Lng - pclat)));// pc+a*(pi-pc) 擴大1.25倍
            }
            return 0;
        }

        static private double astar(double lat0, double lng0, double lat1, double lng1) //A*/////////before
        {
            //(起點,轉折1~4,目標點)


            List<PointLatLngAlt> dll_turnlist = new List<PointLatLngAlt>();

            dll_turnlist.Add(new PointLatLngAlt(lat0, lng0));//起點

            for (int i = 0; i < nflatLength; i++)
            {
                dll_turnlist.Add(new PointLatLngAlt(dll_exnoflylist[i].Lat, dll_exnoflylist[i].Lng, dll_exnoflylist[i].Alt));//起點
            }
            dll_turnlist.Add(new PointLatLngAlt(lat1, lng1));//目標

            //

            double[,] cij = new double[nflatLength + 2, nflatLength + 2];
            double[] tlat = new double[dll_exnoflylist.Count + 2];
            double[] tlng = new double[dll_exnoflylist.Count + 2];
            for (int i = 0; i < dll_exnoflylist.Count; i++)
            {
                tlat[i] = dll_exnoflylist[i].Lat;
                tlng[i] = dll_exnoflylist[i].Lng;
            }

            cij = smalldistance(tlat, tlng);//crs no-fly zone小距離陣列

            //
            double[] h = new double[cij.GetLength(0)];
            for (int i = 0; i < cij.GetLength(0); i++)//H.function
            {
                h[i] = dist(tlat[tlat.Length - 1], tlng[tlng.Length - 1], tlat[i], tlng[i]);//H.Funtion
                if (h[i] == double.MaxValue) h[i] = 0;//如果到達目標點 H.function=0 (自己的距離會無限大 影響F 改0)
            }

            //
            double[,] open = { { 0, 0, 0, 0 } };//arr.row0=0,0,0,0
            double[,] close = { { 0, 0, 0, 0 } };
            double[,] de = { { 0, 0, 0, 0 } };

            //////////////////////////////search target//////////////////////////////////////
            int C = int.MaxValue;
            double[] C_info = new double[4];
            bool reach_target = false;
            double abs = 0;
            double astar_g;
            int asg = 0;

            do
            {
                double f = double.MaxValue;
                int mini_index = int.MaxValue;
                bool noloop = true;

                for (int i = 0; i < open.GetLength(0); i++) //save open.arr [i,3] 
                {
                    if (open[i, 3] < f)  //get F.function min
                    {
                        mini_index = i;
                        f = open[i, 3];
                    }
                }

                for (int i = 0; i < 4; i++) //從open.arr取出 f.min row
                {
                    C_info[i] = open[mini_index, i];
                }
                open = delete(open, mini_index);//刪除open C row
                C = Convert.ToInt32(C_info[0]);

                double[,] m = new double[0, 4];

                for (int i = 0; i < cij.GetLength(0); i++) //{open,close,de,C_info}
                {
                    bool a = false;
                    for (int j = 0; j < de.GetLength(0); j++) //de目前點(不能被擴充)
                    {
                        a |= (de[j, 0] == i);
                    }
                    if (a)
                    { }

                    else
                    {
                        bool b = false;
                        for (int j = 0; j < close.GetLength(0); j++) //close目前點(不能被擴充)
                        {
                            b |= (close[j, 0] == i);
                        }
                        if (b)
                        { }
                        else
                        {
                            bool c = false;
                            int loop = 0;
                            for (int j = 0; j < open.GetLength(0); j++) //open目前點不擴充
                            {
                                c |= (open[j, 0] == i);
                                if (open[j, 0] == i) loop = j;//open目前點重複-> j儲存至loop
                            }
                            if (c)
                            {
                                if (cij[C, i] + C_info[2] + h[i] < open[loop, 3]) //重複點擇優 F < open.F
                                {
                                    open[loop, 0] = i;
                                    open[loop, 1] = C;
                                    open[loop, 2] = cij[C, i] + C_info[2];
                                    open[loop, 3] = cij[C, i] + C_info[2] + h[i];
                                    noloop = false;
                                }
                            }

                            //正常
                            else
                            {
                                if (cij[C, i] == double.MaxValue)
                                { }
                                else
                                {
                                    double[] EX_info = new double[] { i, C, cij[C, i] + C_info[2], cij[C, i] + C_info[2] + h[i] }; //{目前點,來源點,G.function,F.function}
                                    m = into(m, EX_info);
                                }
                            }
                        }
                    }
                }

                if ((m.GetLength(0) == 0) && (noloop))//是否要把C_info加入de(OPEN無擴充&不在loop中)
                {
                    de = into(de, C_info);
                }
                else
                {
                    close = into(close, C_info);
                    for (int i = 0; i < m.GetLength(0); i++)
                    {
                        double[] EX_infor = new double[4];
                        for (int j = 0; j < 4; j++)
                        {
                            EX_infor[j] = m[i, j];
                        }
                        open = into(open, EX_infor);
                    }
                }

                //停止條件
                reach_target = false;
                for (int i = 0; i < open.GetLength(0); i++)
                {
                    reach_target |= (open[i, 0] == tlat.Length - 1);//停止條件.到達目標點
                    if (open[i, 0] == tlat.Length - 1)
                    {
                        abs = open[i, 1];
                        asg = i;
                    }
                }
                astar_g = open[asg, 2];//行經轉折點的G function(總旅行距離)
            } while (!reach_target);

            ////////////////////////////////////////////////////////////////////
            double[] ans = new double[] { tlat.Length - 1, abs };

            do
            {
                int i_close = 0;
                while (!(close[i_close, 0] == abs))
                {
                    i_close++;
                }
                Array.Resize(ref ans, ans.Length + 1);
                ans[ans.Length - 1] = close[i_close, 1];
                abs = close[i_close, 1];

            } while (!(abs == 0));

            for (int i = 0; i < ans.Length; i++)
            {
                astar_ans[i] = Convert.ToInt32(ans[i]);
            }


            return astar_g;


        }

        static private double[,] smalldistance(double[] slat, double[] slng)// 原點.轉折點.目標點陣列
        {
            double[,] cij = new double[nflatLength + 2, nflatLength + 2];//轉折點
            //矩陣
            //double[] latlng = new double[] { lat[0], lng[0], lat[1], lng[1] };
            for (int i = 0; i < nflatLength + 2; i++)
            {
                for (int j = 0; j < nflatLength + 2; j++)
                {
                    bool xx = false;
                    for (int k = 0; k < nflatLength; k++)
                    {
                        xx = xx || crsnofly(slat[i], slng[i], slat[j], slng[j], dll_noflylist[k].Lat, dll_noflylist[k].Lng, dll_noflylist[k + 1].Lat, dll_noflylist[k + 1].Lng);
                    }

                    if (xx == false) cij[i, j] = dist(slat[i], slng[i], slat[j], slng[j]);

                    else cij[i, j] = double.MaxValue;

                }
            }
            return cij;
        }

        static private double[,] delete(double[,] array, int c) //取出後刪除row//<<this
        {
            double[,] array1 = new double[array.GetLength(0), array.GetLength(1)];//<<this
            Array.Copy(array, array1, array.GetLength(0) * array.GetLength(1));

            array = new double[array1.GetLength(0) - 1, 4];
            int h = 0;
            for (int i = 0; i < array.GetLength(0) + 1; i++)
            {
                if (i != c)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        array[i - h, j] = array1[i, j];
                    }
                }
                else
                {
                    h++;
                }
            }
            return array;
        }

        static private double[,] into(double[,] array2d, double[] array1d) //add下方列//<<this
        {
            double[,] array1 = new double[array2d.GetLength(0), array2d.GetLength(1)];//<<this
            Array.Copy(array2d, array1, array2d.GetLength(0) * array2d.GetLength(1));

            array2d = new double[array1.GetLength(0) + 1, 4];
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    array2d[i, j] = array1[i, j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                array2d[array2d.GetLength(0) - 1, i] = array1d[i];
            }
            return array2d;
        }

    }
}
