#include <iostream>
#include <string>

using namespace std;

int getNum (string s, int& cur)
{
    int res = 0;
    int pow = 0;
    while(s[cur + pow] != '[') //cur + pow < s.length() && s[cur + pow]>= '0' && s[cur + pow] <= '9' 这些判定条件在合法的输入下并不需要
    {
        pow ++;
    }
    for(int i = 0; i<pow; i++)
    {
        res = res*10 + s[cur + i] - '0';
    }
    cur += pow;
    return res;
}

string stringParser(string s)
{
    string ret = "";
    int cur = 0;
    while(cur <s.length())
    {
        if(s[cur] >= '0' && s[cur] <= '9')
        {
            int num = getNum(s, cur);
            int bracketNum = 0;
            int substrStart = cur, substrLength = 0;
            while(true)
            {
                substrLength++;
                if(s[cur] == '[') bracketNum ++;
                if (s[cur] == ']') bracketNum --;

                cur++;
                if (bracketNum == 0) break;
            }
            string strRepeated = "";
            if(substrLength>2)
            {
                strRepeated = stringParser(s.substr(substrStart + 1, substrLength-2));
            }
            // cout<<strRepeated<<endl;
            for(int i =0; i <num; i++)
            {
                ret.append(strRepeated);
            }
        }
        else
        {
            ret.push_back(s[cur++]);
        }
    }
    return ret;

}
int main()
{
    int t;
    cin>>t;
    while(t--)
    {
        string s;
        cin>>s;
        cout<<stringParser(s)<<endl;
    }
    return 0;
}
