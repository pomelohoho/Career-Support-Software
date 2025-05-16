// src/pages/RegisterPage.jsx
import React, { useState, useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import {
    Box, Card, CardContent, Avatar,
    TextField, Button, Alert, Typography,
    InputAdornment, Link
} from '@mui/material';
import {
    PersonAdd as PersonAddIcon,
    Email as EmailIcon,
    VpnKey as KeyIcon
} from '@mui/icons-material';
import { createTheme, ThemeProvider } from '@mui/material/styles';

const lightTheme = createTheme({
    palette: {
        mode: 'light',
        primary: { main: '#1976d2' },
        background: { default: '#f5f5f5', paper: '#ffffff' },
        text: { primary: '#333', secondary: '#555' },
    },
});

export default function RegisterPage() {
    const { register } = useContext(AuthContext);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirm, setConfirm] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async e => {
        e.preventDefault();
        setError('');
        setSuccess('');

        if (password !== confirm) {
            setError("Passwords don't match");
            return;
        }

        try {
            await register(email, password);
            setSuccess('Account created! Redirecting to login…');
            setTimeout(() => navigate('/login', { replace: true }), 1500);
        } catch (err) {
            setError(err.message || 'Registration failed');
        }
    };

    return (
        <ThemeProvider theme={lightTheme}>
            <Box
                sx={{
                    minHeight: '100vh',
                    width: '100vw',
                    bgcolor: 'background.default',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    p: 2,
                }}
            >
                <Card sx={{ maxWidth: 400, width: '100%' }}>
                    <CardContent sx={{ textAlign: 'center', pt: 4 }}>
                        <Avatar sx={{ bgcolor: 'primary.main', m: '0 auto', mb: 2 }}>
                            <PersonAddIcon />
                        </Avatar>
                        <Typography variant="h5" gutterBottom>
                            Register
                        </Typography>

                        {error && (
                            <Alert severity="error" sx={{ mb: 2 }}>
                                {error}
                            </Alert>
                        )}
                        {success && (
                            <Alert severity="success" sx={{ mb: 2 }}>
                                {success}
                            </Alert>
                        )}

                        <Box component="form" onSubmit={handleSubmit} noValidate>
                            <TextField
                                label="Email"
                                type="email"
                                required
                                fullWidth
                                margin="normal"
                                value={email}
                                onChange={e => setEmail(e.target.value)}
                                InputProps={{
                                    startAdornment: (
                                        <InputAdornment position="start">
                                            <EmailIcon color="action" />
                                        </InputAdornment>
                                    )
                                }}
                            />

                            <TextField
                                label="Password"
                                type="password"
                                required
                                fullWidth
                                margin="normal"
                                value={password}
                                onChange={e => setPassword(e.target.value)}
                                InputProps={{
                                    startAdornment: (
                                        <InputAdornment position="start">
                                            <KeyIcon color="action" />
                                        </InputAdornment>
                                    )
                                }}
                            />

                            <TextField
                                label="Confirm Password"
                                type="password"
                                required
                                fullWidth
                                margin="normal"
                                value={confirm}
                                onChange={e => setConfirm(e.target.value)}
                                InputProps={{
                                    startAdornment: (
                                        <InputAdornment position="start">
                                            <KeyIcon color="action" />
                                        </InputAdornment>
                                    )
                                }}
                            />

                            <Button
                                type="submit"
                                variant="contained"
                                fullWidth
                                sx={{ mt: 3 }}
                            >
                                Register
                            </Button>
                        </Box>

                        <Typography variant="body2" sx={{ mt: 2 }}>
                            Already have an account?{' '}
                            <Link component={RouterLink} to="/login" underline="hover">
                                Sign In
                            </Link>
                        </Typography>
                    </CardContent>
                </Card>
            </Box>
        </ThemeProvider>
    );
}
