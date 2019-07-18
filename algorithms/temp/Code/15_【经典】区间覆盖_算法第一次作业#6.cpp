//OneNote ¾­µä->Çø¼ä¸²¸Ç
#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;
int cmp(pair <int, int> interval1, pair <int, int >interval2)
{
    return interval1.first<interval2.first;
}
int main()
{
    int n,s,e;
    pair <int, int> myPair;
    cin>>n;
    vector < pair <int ,int> > intervals;
    for(int i=0;i<n;i++)
    {
        cin>>s>>e;
        myPair.first=s;
        myPair.second=e;
        intervals.push_back(myPair);
    }
    sort(intervals.begin(),intervals.end(), cmp);
    s=intervals[0].first;
    e=intervals[0].second;
    for(int i=1;i<n;i++)
    {
        if(e<intervals[i].first)
        {
            cout<<"no";
            return 0;
        }
        else
        {
            if(e<intervals[i].second)
            {
                e=intervals[i].second;
            }
        }
    }
    cout<<s<<' '<<e;
    return 0;
}
