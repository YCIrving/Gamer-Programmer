//OneNote 其他->并查集应用

#include <iostream>

using namespace std;

int pre[1003*1001];
int isOpen[1003*1001];

int convert(int i, int j, int n)
{
    return i*(n+1)+j;
}

int disjoint_find(int n)
{
    int ans=n;
    while(pre[ans]!=ans)
    {
        ans=pre[ans];
    }
    int i=n,j;
    while(pre[i]!=ans)
    {
        j=pre[i];
        pre[i]=ans;
        i=j;
    }
    return ans;
}

void disjointSet_union(int r1, int c1, int r2, int c2, int n)
{
    int root1 = disjoint_find(convert(r1, c1, n));
    int root2 = disjoint_find(convert(r2, c2, n));
    if (root1!=root2)
    {
        pre[root1]=root2;
    }
}

bool disjointSet_cmp(int r1, int c1, int r2, int c2, int n)
{
    if(disjoint_find(convert(r1, c1, n)) == disjoint_find(convert(r2, c2, n)))
    {
        return true;
    }
    else
    {
        return false;
    }
}

void visMap(int n)
{
    cout<<"------preMap---------"<<endl;
    for (int i=0;i<=n+1;i++)
    {
        for(int j=1;j<=n;j++)
        {
            cout<<pre[convert(i,j,n)]<<' ';
        }
        cout<<endl;
    }
    cout<<"------preMap---------"<<endl;
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
    int T,m,n,r,c,ans,flag;
    cin>>T;
    for(int k=0;k<T;k++)
    {
        cin>>n>>m;
        ans=0,flag=0;
        if(m<n)
        {
            for(int i=0;i<m;i++)
            {
                cin>>r>>c;
            }
            cout<<-1<<endl;
            continue;
        }
        for(int i=1;i<=n;i++)
        {
            pre[i]=1;
            isOpen[i]=1;
        }
        for(int i=1;i<=n;i++)
        {
            for(int j=1;j<=n;j++)
            {
                pre[convert(i,j,n)]=convert(i,j,n);
                isOpen[convert(i,j,n)]=0;
            }
        }
        for(int i=n+1,j=1;j<=n;j++)
        {
            pre[convert(i,j,n)]=convert(i,n,n);
            isOpen[convert(i,j,n)]=1;
        }
//        visMap(n);
//        visMap2(n);
        for(int i=0;i<m;i++)
        {
            cin>>r>>c;
            ans++;
            isOpen[convert(r,c,n)]=1;
            if(r-1>=0 && isOpen[convert(r-1,c,n)]==1)
            {
                disjointSet_union(r-1,c,r,c,n);
            }
            if(r+1<=n+1 && isOpen[convert(r+1,c,n)]==1)
            {
                disjointSet_union(r+1,c,r,c,n);
            }
            if(c-1>=1 && isOpen[convert(r,c-1,n)]==1)
            {
                disjointSet_union(r,c-1,r,c,n);
            }
            if(c+1<=n && isOpen[convert(r,c+1,n)]==1)
            {
                disjointSet_union(r,c+1,r,c,n);
            }
            if (flag == 0 && disjointSet_cmp(0,1,n+1,n,n))
            {
                cout<<ans<<endl;
                flag=1;
            }
//            visMap(n);
//            visMap2(n);
        }
        if(flag == 0)
        {
            cout<<-1<<endl;
        }
    }
    return 0;
}
