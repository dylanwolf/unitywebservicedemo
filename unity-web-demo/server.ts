import express from 'express';
import bodyParser from 'body-parser';
import { AddressInfo } from 'net';

// Configuration
const port = process.env.PORT || 3000;

// Build app and API
const app = express();
app.use(bodyParser.json());

app.get('/api/basic', (req, res) => res.send({
    success: true,
    message: 'Hello world!'
}));

app.post('/api/with-params', (req, res) => {
    res.send({
        success: true,
        message: `Hello ${req.body.name}`
    })
});

app.get('/api/failed', (req, res) => {
    try {
        throw {}
    } catch (ex) {
        res.sendStatus(500);
    }
});

app.get('/api/notfound', (req, res) => {
    res.sendStatus(404);
})

// Start server
var server = app.listen(port, () => {
    var addrInfo = server.address() as AddressInfo;
    console.log(`Listening on http://${addrInfo.address}:${addrInfo.port}`);
})