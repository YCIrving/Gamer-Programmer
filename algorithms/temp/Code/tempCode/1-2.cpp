#include <iostream>
#include <string>
using namespace std;

string stringParser(string s)
{
    if (s.length()==0) return s;
    string ret;
    int left=0, right=0, m;
    while(left<s.length())
    {
        m = 1;
        right = left + 1;
        while(right<s.length() && s[right]==s[right-1]+1)
        {
            right ++;
            m++;
        }
        if(m<4)
        {
            for(int i=left; i<right; i++)
            {
                ret.push_back(s[i]);
            }
        }
        else
        {
            ret.push_back(s[left]);
            ret.push_back('-');
            ret.push_back(s[right-1]);
        }
        left = right;
    }
    return ret;
}

int main()
{
    int t;
    cin>>t;
    while(t)
    {
        t--;
        string s;
        cin>>s;
        cout<<stringParser(s)<<endl;
    }
    return 0;
}
