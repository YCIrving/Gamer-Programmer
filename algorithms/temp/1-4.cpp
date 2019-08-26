#include <iostream>
#define MAX_INT 1000000

using namespace std;

//void ShortestPath_Floyd(MGraph MG, Patharc P, ShortPathTable D)
//{
//    int v, w, k;
//    for (v = 0; v < MG.numVertexes; v++)/* 初始化D与P */
//    {
//        for (w = 0; w < MG.numVertexes; w++)
//        {
//            D[v][w] = MG.arc[v][w];/* D[v][w]值即为对应点间的权值 */
//            P[v][w] = w;/* 初始化P */
//        }
//    }
//
//    for (k = 0; k < MG.numVertexes; k++)
//    {
//        for (v = 0; v < MG.numVertexes; v++)
//        {
//            for (w = 0; w < MG.numVertexes; w++)
//            {
//                /* 如果经过下标为k顶点路径比原两点间路径更短 */
//                if (D[v][w] > D[v][k] + D[k][w])
//                {
//                    /* 将当前两点间权值设为更小的一个 */
//                    D[v][w] = D[v][k] + D[k][w];
//                    P[v][w] = P[v][k];/* 路径设置为经过下标为k的顶点 */
//                }
//            }
//        }
//    }
//}
//
//#include <iostream>     // std::cout
//#include <algorithm>    // std::next_permutation, std::sort
//
//int main () {
//  int myints[] = {1,2,3};
//
//  std::sort (myints,myints+3);
//
//  std::cout << "The 3! possible permutations with 3 elements:\n";
//  do {
//    std::cout << myints[0] << ' ' << myints[1] << ' ' << myints[2] << '\n';
//  } while ( std::next_permutation(myints,myints+3) );
//
//  std::cout << "After loop: " << myints[0] << ' ' << myints[1] << ' ' << myints[2] << '\n';
//
//  return 0;
//}

int main()
{
    int mp[MAX_INT][MAX_INT];
    int n, m;
    cin>>n>>m;
    vector<int> path=(n, 0);
    for(int i=0; i<n; i++)
    {
        path[i] = i;
    }
    for(int i=0; i<m; i++)
    {
        int u,v,w;
        cin>>u>>v>>w;
    }

    return 0;
}
