// OneNote ģ��->ȡ����Ϸ

#include <iostream>
#include <iomanip>
using namespace std;

#define MAX(x,y) (x>y?x:y)
double calculate(int n, int k, int w)
//���ֲ���k����1��w��ȡ�����С�ڵ���n�ĸ���
{
    double cur[20000]={1};
    for(int i=1;i<=k+w-1;i++)
    {
        double cur_temp=0.0;
        for(int j=i-1;j>=MAX(0,i-w);j--)
        {
            if(j<k)
                cur_temp+=cur[j]/w;
        }
        cur[i]=cur_temp;
    }
    double ans=0;
    for(int i=k; i<=n;i++)
    {
        ans+=cur[i];
    }
    return ans;

}
int main()
{
    int n,k,w;
    cin>>n>>k>>w;
    cout<<setiosflags(ios::fixed)<<setprecision(5)<<calculate(n,k,w)<<endl;
    return 0;
}
