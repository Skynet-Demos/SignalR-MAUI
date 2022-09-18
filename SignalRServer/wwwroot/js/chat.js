"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
$('#btnSendMessage').prop('disabled', true);

connection.start().then(function () {
	$('#btnSendMessage').prop('disabled', false);
	alert('Connected to ChatHub');
}).catch(function (err) {
	return console.error(err.toString());
});

$('#btnSendMessage').click(function (e) {
	var user = $("#txtUser").val();
	var message = $("#txtMessage").val();
	connection.invoke("SendMessageToAll", user, message).catch(function (err) {
		return console.error(err.toString());
	});

	// clear message input
	$("#txtMessage").val('');

	// focus again to textbox
	$("#txtMessage").focus();

	e.preventDefault();
});

connection.on("ReceiveMessage", function (user, message) {
	var content = `<b>${user} - </b>${message}`;
	$('#messagesList').append(`<li>${content}</li>`);
});