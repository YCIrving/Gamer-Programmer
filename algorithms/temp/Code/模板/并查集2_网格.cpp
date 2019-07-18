// Percolation问题

//root和isOpen要用一维数组，二维在查找时没办法确定下标
#include <iostream>

#define MAXN 1100
using namespace std;

int root[MAXN*MAXN];
int isOpen[MAXN*MAXN];
int go[][2] = {1,0, -1,0, 0,1, 0,-1};

int convert(int r, int c, int n)
{
    return r*n + c;
}

int unionset_find(int x)
{
    int ans=x;
    while(root[ans]!=ans)
    {
        ans=root[ans];
    }
    int temp;
    while(root[x]!=x)
    {
        temp = root[x];
        root[x] = ans;
        x = temp;
    }
    return ans;
}

void unionset_union(int r1, int c1, int r2, int c2, int n)
{
    int root1 = unionset_find(convert(r1, c1, n));
    int root2 = unionset_find(convert(r2, c2, n));
    if (root1!=root2)
    {
        root[root1]=root2;
    }
}

bool unionset_cmp(int r1, int c1, int r2, int c2, int n)
{
    if(unionset_find(convert(r1, c1, n)) == unionset_find(convert(r2, c2, n)))
    {
        return true;
    }
    else
    {
        return false;
    }
}

void init(int n)
{
    for(int j=1;j<=n;j++)
    {
        root[convert(0,j,n)]=convert(0,1,n);
        isOpen[convert(0,j,n)]=true;
        root[convert(n+1,j,n)]=convert(n+1,1,n);
        isOpen[convert(n+1,j,n)]=true;
    }

    for(int i=1;i<=n;i++)
    {
        for(int j=1;j<=n;j++)
        {
            root[convert(i,j,n)]=convert(i,j,n); // 建议赋值为自身，之后另加一个true和false数组
            isOpen[convert(i,j,n)]=false;
        }
    }

}
void visMap(int n)
{
    cout<<"------rootMap---------"<<endl;
    for (int i=0;i<=n+1;i++)
    {
        for(int j=1;j<=n;j++)
        {
            cout<<root[convert(i,j,n)]<<' ';
        }
        cout<<endl;
    }
    cout<<"------rootMap---------"<<endl;
}
void visMap2(int n)
{
    cout<<"------openMap---------"<<endl;
    for (int i=0;i<=n+1;i++)
    {
        for(int j=1;j<=n;j++)
        {
            cout<<isOpen[convert(i,j,n)]<<' ';
        }
        cout<<endl;
    }
    cout<<"------openMap---------"<<endl;
}
int main()
{
    ios::sync_with_stdio(false);
    int t;
    int m,n;
    int r,c, new_r, new_c;
    int cnt, ans, flag;
    cin>>t;
    while(t--)
    {
        cin>>n>>m;
        init(n);
        ans = 0;
        cnt=0; flag=false;
        if(m<n) // 特判
        {
            for(int i=0;i<m;i++)
            {
                cin>>r>>c;
            }
            cout<<-1<<endl;
            continue;
        }
        for(int i=0;i<m;i++)
        {
            cin>>r>>c;
            if(flag == true) continue;
            ans++;
            isOpen[convert(r,c,n)]=true;
            root[convert(r,c,n)] =convert(r,c,n);
            for(int j=0; j<4; j++)
            {
                new_r = r+go[j][0];
                new_c = c+go[j][1];
                if(new_r >=0 && new_r <=n+1 && new_c >=1 && new_c <=n) //注意行列数不同
                {
                    if(isOpen[convert(new_r, new_c, n)])
                    {
                        unionset_union(r,c,new_r,new_c,n);
                    }
                }
            }
            if (flag == 0 && unionset_cmp(0,1,n+1,1,n))
            {

                flag=1;
            }
        }
        if(flag == 0)
        {
            cout<<-1<<endl;
        }
        else
        {
            cout<<ans<<endl;
        }
    }
    return 0;
}
