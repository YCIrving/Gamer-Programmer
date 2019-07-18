// poj 2533 n*logn�ⷨ

#include <iostream>

#define MAXN 1000000
#define INF 100000000

using namespace std;

int a[MAXN], b[MAXN];

//�ö��ַ����ҵ�һ��λ�ã�ʹ��num>b[i-1]
//����num<b[i],����num����b[i]

int Search(int num, int low, int high)
{
    int mid;
    while(low<= high)
    {
        mid = (low + high)/2;
        if(num >= b[mid]) low = mid+1;
        else high = mid-1;
    }
    return low;
}

int dp(int n)
{
    int i, len, pos;
    b[1] = a[1];
    len = 1;
    for(int i =2; i<=n ;i++)
    {
        if(a[i]> b[len]) // ���a[i]��b[]�����е����ֵ������ֱ�Ӳ��뵽����
                         // ��ע����Ŀ�Ƿ������ϸ����������ǣ�������Ϊ>������Ϊ>=
        {
            len = len +1;
            b[len] =a[i];
        }
        else //�ö��ֲ�����b[]�������ҵ���һ����a[i]���λ�ã�����a[i]�������λ��
        {
            pos = Search(a[i], 1, len);
            b[pos] = a[i];
        }
    }
    return len;
}

int main()
{
    int n;
    cin>>n;
    if(n == 0) //���У�0����ʱ���0
    {
        cout<<0;
        return 0;
    }
    for(int i = 1; i<=n; i++)
    {
        cin>>a[i];
    }
    cout<<dp(n)<<endl;
    return 0;
}
