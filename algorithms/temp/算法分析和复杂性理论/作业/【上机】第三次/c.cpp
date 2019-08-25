//Onenote ��̬�滮�������������

#include <iostream>
#include <string>
#include <math.h>

//�����������������ݡ�ÿ�����ݰ���һ�У������������Ȳ�����200���ַ�������ʾ�������С������ַ���֮�������ɸ��ո������
//M�������Ż�
//ע��M�±���ַ����±�Ķ�Ӧ��ϵ����Ҫ��0��λ��
using namespace std;
string s1,s2;

int M[201][201];

int solve(int i, int j)
{
    if(M[i][j]==-1)
    {
        if(i==0 || j==0)
        {
            M[i][j]=0;
        }
        else
        {
            if(s1[i-1]==s2[j-1])
            {
                M[i][j]=1+(solve(i-1,j-1));
            }
            else
            {
                M[i][j]=max(solve(i,j-1),solve(i-1,j));
            }
        }
    }
    return M[i][j];

}
int main()
{
    int ans=0;
    ios::sync_with_stdio(false);
    while(cin>>s1>>s2)
    {
        for(int i=0;i<=s1.length();i++)
        {
            for(int j=0;j<=s2.length();j++)
            {
                M[i][j]=-1;
            }
        }
        ans=solve(s1.length(),s2.length());
        cout<<ans<<endl;
//        cout<<s1<<endl<<s2<<endl;

    }

}
