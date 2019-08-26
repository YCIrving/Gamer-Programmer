#include <iostream>

using namespace std;

int nums[20010] = {0};
int root[20010];
int cnt[20010] = {0};

int unionSetFind(int x)
{
    int temp = x, ans;
    while(root[temp]!= temp)
    {
        temp = root[temp];
    }
    ans = temp;
    while(root[x]!=ans)
    {
        temp = x;
        root[x] = ans;
        x = temp;
    }
    return ans;
}

void unionSetUnion(int x, int y)
{
    int rootX = unionSetFind(x);
    int rootY = unionSetFind(y);
    if(rootX != rootY)
    {
        root[rootY] = rootX;
    }
}

int LCD(int x, int y)
{
    if(x==0 || y == 0) return 0;
    while(y!=0)
    {
        x = x%y;
        x = x+y;
        y = x-y;
        x = x-y;
    }
    return x;
}

int main()
{
    int n;
    int ans=0;
    cin>>n;
    for(int i=0; i<n; i++)
    {
        root[i] = i;
    }
    for(int i=0; i<n; i++)
    {
        cin>>nums[i];
        for(int j=i-1; j>=0; j--)
        {
            if(LCD(nums[i], nums[j])>1)
            {
                unionSetUnion(i, j);
            }
        }
    }

    for(int i=0; i<n; i++)
    {
        cnt[root[i]]++;
        if(cnt[root[i]]>ans) ans = cnt[root[i]];
    }
    cout<<ans<<endl;

    return 0;
}

/*

14
20 50 22 74 9 4 21 32 34 5467 88903 372837 983 2345

6
0 50 22 74 9 63

*/
