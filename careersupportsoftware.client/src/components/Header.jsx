// src/components/Header.jsx
import React, { useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { Box, Button, Typography } from '@mui/material';

export default function Header() {
    const { logout } = useContext(AuthContext);

    return (
        <Box
            sx={{
                width: '100%',
                p: 2,
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                bgcolor: 'primary.main',
                color: 'white'
            }}
        >
            <Typography variant="h6">Visa Jobs</Typography>
            <Button onClick={logout} variant="outlined" color="inherit">
                Log Out
            </Button>
        </Box>
    );
}
