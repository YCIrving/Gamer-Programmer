#include <iostream>
#include <algorithm>

using namespace std;

struct TimeSlot
{
    double c;
    double e;
    double v;
}timeSlot[100010];

bool cmp(TimeSlot ts1, TimeSlot ts2)
{
    return ts1.v>ts2.v;
}

char judge(int a, int b, int s)
{
    for(int i=0; i<s; i++)
    {
        if(a!=0)
        {
            if(a - timeSlot[i].c > 0)
            {
                a-= timeSlot[i].c;
                continue;
            }
            else
            {
                double f = a/timeSlot[i].c;
                a = 0;
                b -= (1-f) * timeSlot[i].e;
                if(b<=0) return 'Y';
            }
        }
        else
        {
            if(b!= 0)
            {
                if(b - timeSlot[i].e > 0)
                {
                    b-= timeSlot[i].e;
                    continue;
                }
                else
                {
                    b = 0;
                    return 'Y';
                }
            }
            else return 'Y';
        }
    }
    return 'N';

}
int main()
{
    int t;
    cin>>t;
    for(int caseID =1; caseID<=t; caseID++)
    {
        string ans = "";
        int d, s;
        int a, b;
        cin>>d>>s;
        for(int i=0;i<s;i++)
        {
            cin>>timeSlot[i].c>>timeSlot[i].e;
            //timeSlot[i].v = timeSlot[i].c * timeSlot[i].c / (timeSlot[i].c+timeSlot[i].e);
            timeSlot[i].v = timeSlot[i].c / timeSlot[i].e;
        }
        sort(timeSlot, timeSlot+s, cmp);

//        for(int i=0; i<s; i++)
//        {
//            cout<<timeSlot[i].v<<' '<<timeSlot[i].c<<' '<<timeSlot[i].e<<endl;
//        }

        for(int i=0; i<d; i++)
        {
            cin>>a>>b;
            ans.push_back(judge(a, b, s));
        }

        cout<<"Case #"<<caseID<<": "<<ans<<endl;
    }
    return 0;
}

/*
2
4 2
3 8
6 10
0 18
3 13
10 0
7 3
1 2
4 4
4 4
0 0
*/
