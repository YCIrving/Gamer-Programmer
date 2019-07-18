// OneNote ����->��ƫ�ƵĲ��鼯

#include <iostream>
#include <vector>
#include <algorithm>
#include <stdio.h>

using namespace std;

struct Node
{
    int parent;
    int relation;
}arr[50001];

//�ҵ��ýڵ�ĸ��ڵ��������ڵ�Ĺ�ϵ
pair<int, int> disjointSetFind(int x)
{
    int root_x=x,relation_x=0,root_temp,relation_temp,relation_current;
    while(arr[root_x].parent!=root_x)
    {
        relation_x=(relation_x+arr[root_x].relation)%3;
        root_x=arr[root_x].parent;
    }
    relation_current=relation_x;

    //Path compression

    while(arr[x].parent!=root_x)
    {
        root_temp=arr[x].parent;
        relation_temp=arr[x].relation;
        arr[x].parent=root_x;
        arr[x].relation=relation_current;
        x=root_temp;
        relation_current=(relation_current-relation_temp+3)%3;
    }
    return make_pair(root_x,relation_x);
}

//�������ڵ����ڵļ��Ͻ��кϲ�,d��ʾ�����ڵ�Ĺ�ϵ
int disjointSetUnion(int d, int x, int y)
{
    pair<int, int>x_root,y_root;
    x_root=disjointSetFind(x);
    y_root=disjointSetFind(y);
    if(x_root.first != y_root.first)
    {
        arr[x_root.first].parent=y_root.first;
        arr[x_root.first].relation=(d+y_root.second-x_root.second+3)%3;
    }
    return 0;
}
int judgeEatSpecies(int x, int y)
{
    pair<int, int> x_root,y_root;
    x_root=disjointSetFind(x);
    y_root=disjointSetFind(y);
    if (x_root.first!=y_root.first)
    {
        disjointSetUnion(1,x,y);
    }
    else
    {
        //first ��ͬ
        if(((x_root.second-y_root.second)+3)%3!=1)
        {
            return 1;
        }
    }
    return 0;
}

int judgeSameSpecies(int x, int y)
{
    if(x==y)
    {
        return 0;
    }
    else
    {
        pair<int, int> x_root,y_root;
        x_root=disjointSetFind(x);
        y_root=disjointSetFind(y);
        if (x_root.first!=y_root.first)
        {
            disjointSetUnion(0,x,y);
        }
        else
        {
            if(x_root.second!=y_root.second)
            {
                return 1;
            }
        }
    }
    return 0;
}
int judge(int d, int x, int y)
{
    if(d==1)
    {
        return judgeSameSpecies(x,y);
    }
    return judgeEatSpecies(x,y);
}
int main()
{
    ios::sync_with_stdio(false);
    for(int i=0;i<50001;i++)
    {
        arr[i].parent = i;
        arr[i].relation = 0;
    }
    int n,k,d,ans=0;
    int x,y;
    scanf("%d%d",&n,&k);
//    cin>>n>>k;
    for(int cnt=1;cnt<=k;cnt++)
    {
//        cout<<"ans: "<<ans<<endl;
//        for(int i =1;i<=n;i++)
//        {
//            cout<<i<<": "<<arr[i].parent<<", "<<arr[i].relation<<endl;
//        }
//        cin>>d>>x>>y;
        scanf("%d%d%d",&d,&x,&y);
        if(x>n || y>n)
        {
            ans++;
            continue;
        }
        if(d==2 && x==y)
        {
            ans++;
            continue;
        }
        ans += judge(d,x,y);
    }
//    cout<<ans<<endl;
    printf("%d\n",ans);
}
