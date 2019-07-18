// poj 2533 n*logn解法

#include <iostream>

#define MAXN 1000000
#define INF 100000000

using namespace std;

int a[MAXN], b[MAXN];

//用二分法查找到一个位置，使得num>b[i-1]
//并且num<b[i],并用num代替b[i]

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
        if(a[i]> b[len]) // 如果a[i]比b[]数组中的最大值还大，则直接插入到后面
                         // 另，注意题目是否是求严格递增，如果是，则这里为>，否则为>=
        {
            len = len +1;
            b[len] =a[i];
        }
        else //用二分查找在b[]数组中找到第一个比a[i]大的位置，并让a[i]代替这个位置
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
    if(n == 0) //特判：0个数时输出0
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
