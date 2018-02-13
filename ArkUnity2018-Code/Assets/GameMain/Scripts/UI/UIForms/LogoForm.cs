using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * asjdfkjasd;g
 * adfsaghdfhgf
 */
public class LogoForm : UGuiForm {

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        string s = string.Format(@"* This source file is part of ArkGameFrame                
* For the latest info, see https://github.com/ArkGame
*
* Copyright (c) 2013-2017 ArkGame authors.
*
* Licensed under the Apache License, Version 2.0 (the {0});
* you may not use this file except in compliance with the License.
*You may obtain a copy of the License at
*
*http://www.apache.org/licenses/LICENSE-2.0
*
*Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an {1} BASIS,
*WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/","\"License\"", "\"AS IS\"");
        Debug.Log(s);
        //        string tmp = @"/*                                
        //* This source file is part of ArkGameFrame                
        //* For the latest info, see https://github.com/ArkGame
        //*
        //* Copyright (c) 2013-2017 ArkGame authors.
        //*
        //* Licensed under the Apache License, Version 2.0 (the 'License');
        //* you may not use this file except in compliance with the License.
        //*You may obtain a copy of the License at
        //*
        //*http://www.apache.org/licenses/LICENSE-2.0
        //*
        //*Unless required by applicable law or agreed to in writing, software
        //* distributed under the License is distributed on an 'AS IS' BASIS,
        //*WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
        //* See the License for the specific language governing permissions and
        //* limitations under the License.
        //*
        //*/ ";
        //        Debug.Log(tmp);


    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);
    }
}
