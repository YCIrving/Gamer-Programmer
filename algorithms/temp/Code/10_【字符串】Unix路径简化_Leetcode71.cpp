//OneNote ×Ö·û´®->UnixÂ·¾¶¼ò»¯
#include <iostream>
#include <string>
#include <vector>
using namespace std;
int main()
{
    string str,word(4096,'\0');
    int wordL=0;
    vector <string> s;
    getline(cin, str);
    char lastC='\0', C='\0';
    for(int i=0;i<str.length();i++)
    {
        C=str[i];
        if(C=='/')
        {
            if(wordL!=0)
            {
                string word_temp(word.begin(),word.begin()+wordL);
                s.push_back(word_temp);
                wordL=0;
            }
        }
        else if(C=='.')
        {
            if(lastC=='.')
            {
                if(s.size()>0)
                    s.pop_back();
            }
        }
        else
        {
            word[wordL]=C;
            wordL++;
        }
        lastC=C;
    }
    if(wordL!=0)
    {
        string word_temp(word.begin(),word.begin()+wordL);
        s.push_back(word_temp);
    }
    if(s.size()==0)
    {
        cout<<'/';
        return 0;
    }

    for(int i=0;i<s.size();i++)
    {
        cout<<'/'<<s[i];
    }
}
