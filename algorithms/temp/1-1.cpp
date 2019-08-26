/*
2
6.6.6 6.6.7
1 1.0

true
false

*/

#include <iostream>
#include <string>
#include <vector>

using namespace std;

vector<int> split(string s)
{
    vector<int> ret;
    int i =0;

    while(i<s.length())
    {
        int ans = 0;
        while(s[i]!='.' && s[i]!= '\0')
        {
            ans = ans*10 + s[i] - '0';
            i++;
        }
        ret.push_back(ans);
        i++;
    }

    for(i=ret.size(); i<4; i++)
    {
        ret.push_back(0);
    }
    return ret;
}

bool compare(vector<int> nums1, vector<int> nums2)
{
    for(int i=0; i<4; i++)
    {
        if(nums1[i]<nums2[i]) return true;
        else if(nums1[i]>nums2[i]) return false;
        else continue;
    }
    return false;
}
int main()
{
    int t;
    cin>>t;
    while(t--)
    {
        string s1, s2;
        cin>>s1>>s2;
        vector<int> nums1 = split(s1);
        vector<int> nums2 = split(s2);
//        for(int i=0; i<4; i++)
//        {
//            cout<<nums1[i]<<' ';
//        }
//        cout<<endl;
//        for(int i=0; i<4; i++)
//        {
//            cout<<nums2[i]<<' ';
//        }
//        cout<<endl;
        if(compare(nums1, nums2)) cout<<"true"<<endl;
        else cout<<"false"<<endl;
    }
    return 0;
}
