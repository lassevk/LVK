Write-Host "Opening SSH Connection to forward port 33048 to Gandalf, REMEMBER TO DISCONNECT"
ssh -L 33048:127.0.0.1:3306 "$($Env:GANDALF_USERNAME)@$($Env:GANDALF_HOSTNAME)" -p $Env:GANDALF_PORT
Write-Host "Forwarded ports closed"