//Onenote ��̬�滮�����������
#include <iostream>
#include <math.h>
#include <algorithm>
#include <memory.h>
#define MAX 110
#define INF 100000000
using namespace std;

int opt[MAX][MAX]={0};
int a[MAX];
int main()
{
    int n;
    cin>>n;
    for(int i=0; i<n; i++)
    {
        cin>>a[i];
    }
    int min_num;
    for(int l = 3; l<=n; l++) //����
    {
        for(int i = 0; i<= n-l; i++) //��ʼ�㣬ȷ������
        {
            min_num = INF;
            for(int j = i+1; j<= i+l-2; j++ ) // ��������ѭ��
            {
                min_num  = min(min_num, opt[i][j] + opt[j][i+l-1] + a[i] * a[j] * a[i+l-1]);
//                cout<<opt[i][j] << ' '<<opt[j][i+l-1]<< ' '<< a[j-1]<<' '<< a[j]<< ' '<<a[j+1]<<'='<< min_num<<endl;
            }
            opt[i][i+l-1] = min_num;
//            cout<<min_num<<endl;
        }
    }
    cout<<opt[0][n-1]<<endl;
    return 0;
}
