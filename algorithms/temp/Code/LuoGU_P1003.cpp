/*����ϵ�һ��1Y����Ȼ֮ǰ������Ŀ�Ƚϼ򵥣����Ƕ�û��һ�γɹ���
���Ի��Ǻܾ�������ܹ�1Y��
����������˼���ǿ�����̳��������ģ���Ϊ�Լ���˼·������һ���൱�ڵذ�Ķ�ά����
Ȼ����ݽ���̺���ǵ�����ֵΪ��̺��ţ��ᱬ��������ֱ�ӷ���ʵ�֣�����һ����̳��֪������
����ȷ˼·���Ȱѵ�̺����������洢������Ȼ���������һ�ŵ�̺��ʼ��֤���Ƿ��串�Ǽ��ɡ�
*/

#include<iostream>
using namespace std;
int main()
{
    int floor[100000][4]={0};
    int n,a,b,ans;
    cin>>n;
    for(int i=0;i<n;i++)
    {
        cin>>floor[i][0]>>floor[i][1]>>floor[i][2]>>floor[i][3];
    }
    cin>>a>>b;
    for(ans=n-1;ans>=0;ans--)
    {
        if(a>=floor[ans][0]&&a<=floor[ans][0]+floor[ans][2]&&b>=floor[ans][1]&&b<=floor[ans][1]+floor[ans][3])
            break;
    }
    if(ans==-1)
        cout<<ans;
    else
    cout<<++ans;
    return 0;
}
