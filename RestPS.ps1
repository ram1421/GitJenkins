$acctname = "";
$password = "";
$server = "";
$port = "8080";
$jobName = "FreeStyleJenkins";
$jobToken = "admin";
 
 
$params = @{uri = "http://${server}:${port}/job/${jobName}/build?token=${jobToken}";
                   Method = 'Get';
                   Headers = @{Authorization = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("$(${acctname}):$(${password})"));}
           }
 
$var = invoke-restmethod @params
