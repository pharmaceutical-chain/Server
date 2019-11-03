$result = Invoke-WebRequest -Uri "https://pharmachain.au.auth0.com/oauth/token" -Headers @{"content-type"="application/json"} -Body '{"client_id":"hI6LoWtu7nB4UhbpPDz0mwFMsb5xdEU6","client_secret":"TMI6bNPXmt1BlT57YClbV5pzqcnm-2yzuVA7bJmnJfjSDcZW6cq0tJe1AOoSj6Fq","audience":"https://pharmachain-api.net","grant_type":"client_credentials"}' -Method POST

$accessToken = ($result.Content | ConvertFrom-Json).access_token

Write-Host($accessToken)