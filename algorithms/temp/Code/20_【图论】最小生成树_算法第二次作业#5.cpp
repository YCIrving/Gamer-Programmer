//OneNote 图论->最小生成树

#include <iostream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

bool cmp(pair< pair<int, int>, int> p1, pair< pair<int, int>, int> p2)
{
    return p1.second<p2.second;
}

int solve(vector<pair< pair<int ,int>, int> > &edge_list, int n)
{
    int farms_tag[101]={0};
    int cnt = 0, s, e, c, flag, ans=0;
    farms_tag[0]=1;
    while(cnt!=n)
    {
        for (int i=0;i<edge_list.size();i++)
        {
            pair< pair<int ,int>, int> my_edge = edge_list[i];
            s=my_edge.first.first, e=my_edge.first.second, c=my_edge.second;
            if(farms_tag[s]==0 && farms_tag[e]==1 || farms_tag[s]==1 && farms_tag[e]==0)
            {
                edge_list.erase(edge_list.begin()+i);
                farms_tag[s]=1;
                farms_tag[e]=1;
                ans += c;
                break;
            }
        }
        cnt++;
    }
    return ans;
}

int main()
{
    ios::sync_with_stdio(false);
    int n;
    while(cin>>n)
    {
        vector<pair< pair<int ,int>, int> > edge_list;
        int ans=0,c;
        // read inputs
        for(int i=0;i<n;i++)
        {
            for(int j=0;j<n;j++)
            {
                cin>>c;
                if(i>=j)
                    continue;
                else
                {
                    edge_list.push_back(make_pair(make_pair(i,j),c));
                }
            }
        }
        sort(edge_list.begin(), edge_list.end(), cmp);
//        for(int i=0;i<edge_list.size();i++)
//        {
//            cout<<edge_list[i].first.first<<"->"<<edge_list[i].first.second<<": "<<edge_list[i].second<<endl;
//        }
        ans = solve(edge_list, n);
        cout<<ans<<endl;
    }
    return 0;
}
