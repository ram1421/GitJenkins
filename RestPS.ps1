$acctname = "admin";
$password = "787c18fb8fbc2d03e998285fdcd4a2c0";
$server = "192.168.0.24";
$port = "8080";
$jobName = "FreeStyleJenkins";
$jobToken = "admin";
 
 
$params = @{uri = "http://${server}:${port}/job/${jobName}/build?token=${jobToken}";
                   Method = 'Get';
                   Headers = @{Authorization = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("$(${acctname}):$(${password})"));}
           }
 
$var = invoke-restmethod @params