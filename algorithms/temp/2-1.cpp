/*

3
0 4 0
4 0 6
0 6 0

1

3
0 4 0
4 0 0
0 0 0

2

*/
#include <iostream>

#define MAX_INT 210

using namespace std;

int root[MAX_INT];

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

int main()
{
    int n;
    cin>>n;
    for(int i=0; i<n; i++)
    {
        root[i] = i;
    }

    int times;
    for(int i=0; i<n; i++)
    {
        for(int j=0; j<n; j++)
        {
            cin>>times;
            if(times >= 3)
            {
                unionSetUnion(i, j);
            }
        }
    }
    int ans = 0;

    for(int i=0; i<n; i++)
    {
        if(root[i] == i) ans++;
    }
    cout<<ans<<endl;
    return 0;

}
