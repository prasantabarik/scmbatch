To run producer
	Goto DeliveryMomentMessageGenerator folder run the below command
	dapr run --app-id deliverymoment --dapr-http-port 3500 -d .\components

Run the DeliveryMomentMessageGenerator exe

To run comsumer:
	Goto DeliveryMomentProcesserApi folder 
	dapr run --app-id $Default --port 3501 --app-port 5000 --components-path components dotnet run


