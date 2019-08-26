/*

1 2 3 4 5 6 7 8 9
a b c

1 2 3 4 a 5 6 7 8 b 9 c

*/

#include <iostream>
#include <string>

using namespace std;

string stringParser(string s1, string s2)
{
    int pos1=0, pos2=0;
    int cnt1=0, cnt2=0;
    string ans="";
    while(pos1!=s1.length() && pos2!=s2.length())
    {
        cnt1= 0;
        while(cnt1!=4 && pos1!=s1.length())
        {
            //cout<<"haha2";
            if(s1[pos1] == ' ') cnt1++;
            ans.push_back(s1[pos1]);
            pos1++;
        }
        if(pos1==s1.length()) break;
        cnt2 = 0;
        while(cnt2!=1 && pos2 != s2.length())
        {
            //cout<<"haha3";
            if(s2[pos2] == ' ') cnt2++;
            ans.push_back(s2[pos2]);
            pos2++;
        }
        if(pos2 == s2.length()) break;
    }

    if(pos1!=s1.length())
    {
        ans.push_back(' ');
        while(pos1!=s1.length())
        {
            ans.push_back(s1[pos1]);
            pos1++;
        }
    }
    if(pos2!=s2.length())
    {
        ans.push_back(' ');
        while(pos2!=s2.length())
        {
            ans.push_back(s2[pos2]);
            pos2++;
        }
    }

    return ans;
}

int main()
{
    string s1, s2;
    getline(cin, s1);
    getline(cin, s2);
    cout<<stringParser(s1, s2);
    return 0;
}
