var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(4567);

io.on('connection', function(socket){
	socket.on('beep', function(){
		socket.emit('boop',{ x: Math.random() * 10, y: Math.random() * 10, z: Math.random() * 10 });
	});
	socket.on('ping', function(data) {
		console.log(data);
		socket.emit('ping', data ||{})
	})
})
