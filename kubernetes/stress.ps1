$url = "http://localhost/api/Orders/6b5c2c35-0c26-4546-a0a9-cacbc5e9893d/status"

$headers = @{
  "Content-Type" = "application/json"
}

$data = @{
  status   = "Completed"
}

$json = $data | ConvertTo-Json

$numberOfRequests = 1000

for ($i = 1; $i -le $numberOfRequests; $i++) {
  $response = Invoke-WebRequest -Uri $url -Method Patch -Headers $headers -Body $json
  Write-Host "Request: $i - Status Code: $($response.StatusCode)"
}
