# GitJenkins
Trigger Jenkins build

Steps:
1. Github repo  will send a email notification to configured email
2. Upon receiving the email, this service GitJenkins will read the email
3. Validate if this email is not spam by checking the secret and from
4. If the email is valid, get the reponame from the email subject
5. Send a post request using jenkins URL to build the pipeline
https://sessum.serveo.net/login?from=%2F
